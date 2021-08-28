using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : MonoBehaviour
{
    private Rigidbody rigidbody;
    protected Animator animator;
    [SerializeField] public float speed;

    private bool attackQueued = false;
    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;
    public Transform attackPoint;
    private float attackVolume = 1.5f;
    private Collider[] targets;
    public Arrow ArrowPrefab;
    private Rigidbody ArrowRB;
    public GameObject Win, Lose;

    public int maxHealth;
    [HideInInspector] public int health;

    [SerializeField] GameObject ragdollPrefab;

    [SerializeField] Camera TopDownCamera;

    private Arrow currentArrow;

    bool shoot;
    public float shootPower;
    public float maxShootPower;
    public float shootPowerSpeed;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        health = maxHealth;
        reload();
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            Move();
            Rotate();

            if (Input.GetMouseButtonDown(0)) shoot = true;

            if(shoot && shootPower < maxShootPower)
            {
                shootPower += Time.deltaTime * shootPowerSpeed;
            }

            if (shoot && Input.GetMouseButtonUp(0))
            {
                Shoot(shootPower * 5);
                shootPower = 15;
                shoot = false;
            }
        }
    }

    private void Move()
    {
        rigidbody.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * speed, rigidbody.velocity.y, Input.GetAxisRaw("Vertical") * speed);
        Vector3 xzvelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        animator.SetFloat("Speed", xzvelocity.sqrMagnitude);
    }

    private void Rotate()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo,
                Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable"));
            transform.LookAt(new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z));
        }
        else
            transform.LookAt(transform.position + rigidbody.velocity);
    }

    private void reload()
    {
        if (alreadyAttacked || currentArrow != null) return;
        alreadyAttacked = true;
        StartCoroutine(RealoadAfterTime());
    }

    private IEnumerator RealoadAfterTime()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        currentArrow = Instantiate(ArrowPrefab, attackPoint);

        currentArrow.transform.localPosition = Vector3.zero;
        alreadyAttacked = false;
    }

    public void Shoot(float shootPower)
    {
        if (alreadyAttacked || currentArrow == null) return;


        //currentArrow.Fly(attackPoint.TransformDirection(Vector3.forward * shootPower * Time.deltaTime));
        currentArrow.Fly(attackPoint.forward * shootPower);
        currentArrow = null;

        reload();
    }

    public bool isReady()
    {
        return (!alreadyAttacked && currentArrow != null);
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hit");

        health -= damage;

        if (health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        Debug.Log("Game over");
        Lose.SetActive(true);
        gameObject.SetActive(false);
        GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
    }

    private void Heals(int healthA)
    {
        health += healthA;
        if (health > maxHealth)
            health = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "heals")
        {
            Heals(30);
            other.gameObject.SetActive(false);
        }
    }
}
