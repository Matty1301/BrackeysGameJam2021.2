using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Animator animator;
    public float speed;

    [SerializeField] public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public Transform attackPoint;
    private float attackVolume = 1.5f;
    private Collider[] targets;
    public GameObject Win, Lose;

    [SerializeField] public int weaponDamage;

    public int maxHealth;
    [HideInInspector] public int health;

    [SerializeField] GameObject ragdollPrefab;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
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
        if ((Input.GetButtonUp("AttackL") || Input.GetButtonUp("AttackR")) && !alreadyAttacked)
        {
            alreadyAttacked = true;
            animator.ResetTrigger("Hit");
            animator.SetTrigger("Attack" + (Random.Range(1, 6)));
            Invoke("RegisterHits", 0.5f);
            Invoke("ResetAttack", timeBetweenAttacks);
        }
    }

    public void RegisterHits()
    {
        if (gameObject.activeInHierarchy)
        {
            targets = Physics.OverlapSphere(attackPoint.position, attackVolume, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (Collider target in targets)
            {
                if (target.GetComponent<CharacterController>() != null)
                    target.GetComponent<CharacterController>().TakeDamage(weaponDamage);
            }
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
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
        if(other.gameObject.tag == "heals")
        {
            Heals(30);
            other.gameObject.SetActive(false);
        }
    }
}
