using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : MonoBehaviour
{
    private Rigidbody rigidbody;
    [SerializeField] public float speed;

    [SerializeField] private float timeBetweenAttacks;
    private bool alreadyAttacked;
    public Transform attackPoint;
    private float attackVolume = 1.5f;
    private Collider[] targets;
    public GameObject PrefabFireBall;
    private Rigidbody FireBallRB;
    public GameObject Win, Lose;

    [SerializeField] private int weaponDamage;

    public int maxHealth;
    [HideInInspector] public int health;

    [SerializeField] GameObject ragdollPrefab;

    [SerializeField] Camera TopDownCamera;
    public float AttackSpread;
    public float AttackForce;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        health = maxHealth;
    }

    private void Update()
    {
        Move();
        Rotate();
        Attack();
    }

    private void Move()
    {
        rigidbody.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * speed, rigidbody.velocity.y, Input.GetAxisRaw("Vertical") * speed);
        Vector3 xzvelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
    }

    private void Rotate()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable"));
            transform.LookAt(new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z));
        }
        else
            transform.LookAt(transform.position + rigidbody.velocity);
    }

    private void Attack()
    {
        if ((Input.GetButtonDown("AttackL") || Input.GetButtonDown("AttackR")) && !alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke("ThrowBall", 0.3f);
            Invoke("ResetAttack", timeBetweenAttacks);
        }
    }

    public void ThrowBall()
    {
        /*
        Ray ray = TopDownCamera.ViewportPointToRay(Input.mousePosition);
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        float x = Random.Range(-AttackSpread, AttackSpread);
        float y = Random.Range(-AttackSpread, AttackSpread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        GameObject currentBall = Instantiate(PrefabFireBall, attackPoint.position, Quaternion.identity);
        currentBall.transform.forward = directionWithSpread.normalized;

        FireBallRB = currentBall.GetComponent<Rigidbody>();
        FireBallRB.AddForce(directionWithSpread.normalized * AttackForce, ForceMode.Impulse);
    */
        
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable"));
        Instantiate(PrefabFireBall, attackPoint.transform.position, Quaternion.identity).transform.LookAt(new Vector3(hitInfo.point.x, attackPoint.transform.position.y, hitInfo.point.z));

    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {

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
