using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardController : Controller
{
    private Rigidbody rigidbody;
    //protected Animator animator;
    protected AudioSource audioSource;
    //public float speed;

    [SerializeField] protected AudioClip[] fireballAttackSounds, fireballAttackSounds2, eatingMeatSounds;

    private bool attackQueued = false;
    [SerializeField] public float timeBetweenAttacks;
    private bool alreadyAttacked;
    //public Transform attackPoint;
    private float attackVolume = 1.5f;
    private Collider[] targets;
    public GameObject PrefabFireBall;
    private Rigidbody FireBallRB;
    //public GameObject Win, Lose;

    [SerializeField] private int weaponDamage;

    //public int maxHealth;
    //[HideInInspector] public int health;

   //[SerializeField] GameObject ragdollPrefab;

    [SerializeField] Camera TopDownCamera;
    public float AttackSpread;
    public float AttackForce;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        health = maxHealth;
    }

    private void Update()
    {
        if (Time.timeScale != 0)
        {
            Move();
            Rotate();
            if (Input.GetButtonDown("AttackL") || Input.GetButtonDown("AttackR"))
                Attack();
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

    private void Attack()
    {
        if (alreadyAttacked == false)
        {
            alreadyAttacked = true;
            Invoke("PlayAttackSound", 0.1f);
            animator.ResetTrigger("Hit");
            animator.SetTrigger("Attack");
            Invoke("ThrowBall", 0.4f);
            Invoke("ResetAttack", timeBetweenAttacks);
        }

        else if (attackQueued == false)
            attackQueued = true;
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

        //Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable"));
        //Instantiate(PrefabFireBall, attackPoint.transform.position, Quaternion.identity).transform.LookAt(new Vector3(hitInfo.point.x, attackPoint.transform.position.y, hitInfo.point.z));
        Instantiate(PrefabFireBall, transform.position, transform.rotation);
    }

    protected void ResetAttack()
    {
        alreadyAttacked = false;
        if (attackQueued == true)
        {
            Attack();
            attackQueued = false;
        }
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
        if (other.gameObject.tag == "heals" && health < maxHealth)
        {
            audioSource.PlayOneShot(eatingMeatSounds[Random.Range(0, eatingMeatSounds.Length)]);
            Heals(30);
            other.gameObject.SetActive(false);
        }
    }

    protected void PlayAttackSound()
    {
        audioSource.PlayOneShot(fireballAttackSounds[Random.Range(0, fireballAttackSounds.Length)]);
        audioSource.PlayOneShot(fireballAttackSounds2[Random.Range(0, fireballAttackSounds2.Length)]);
    }
}
