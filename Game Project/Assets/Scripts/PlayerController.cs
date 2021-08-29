using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    protected Rigidbody rigidbody;
    //protected Animator animator;
    protected AudioSource audioSource;
    //public float speed;

    [SerializeField] protected AudioClip[] swordAttackSounds, maceAttackSounds, axeAttackSounds, eatingMeatSounds;

    //[HideInInspector] public int currentWeapon;

    private bool attackQueued = false;
    [SerializeField] protected float timeBeforeRegisterHits;
    [SerializeField] public float timeBetweenAttacks;
    protected bool alreadyAttacked;
    //public Transform attackPoint;
    protected float attackVolume = 1.5f;
    protected Collider[] targets;
    //public GameObject Win, Lose;

    [SerializeField] public int weaponDamage;

    //public int maxHealth;
    //[HideInInspector] public int health;

    //[SerializeField] protected GameObject ragdollPrefab;

    protected void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        health = maxHealth;
        currentWeapon = GetComponent<SetWeaponMelee>().currentWeapon;
    }

    protected void Update()
    {
        if (Time.timeScale != 0)
        {
            Move();
            Rotate();
            if (Input.GetButtonDown("AttackL") || Input.GetButtonDown("AttackR"))
                Attack();
        }
    }

    protected void Move()
    {
        rigidbody.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * speed, rigidbody.velocity.y, Input.GetAxisRaw("Vertical") * speed);
        Vector3 xzvelocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        animator.SetFloat("Speed", xzvelocity.sqrMagnitude);
    }

    protected void Rotate()
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

    protected virtual void Attack()
    {
        if (alreadyAttacked == false)
        {
            alreadyAttacked = true;
            Invoke("PlayAttackSound", 0.1f);
            animator.ResetTrigger("Hit");
            animator.SetTrigger("Attack" + (Random.Range(1, 6)));
            Invoke("RegisterHits", timeBeforeRegisterHits);
            Invoke("ResetAttack", timeBetweenAttacks);
        }

        else if (attackQueued == false)
            attackQueued = true;
    }

    public void RegisterHits()
    {
        if (gameObject.activeInHierarchy)
        {
            targets = Physics.OverlapSphere(attackPoint.position, attackVolume, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (Collider target in targets)
            {
                if (target.GetComponent<CharacterController>() != null)
                {
                    target.GetComponent<CharacterController>().TakeDamage(weaponDamage);
                }
            }
        }
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

    protected void Death()
    {
        Debug.Log("Game over");
        Lose.SetActive(true);
        gameObject.SetActive(false);
        GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
    }
    

    protected void Heals(int healthA)
    {
        health += healthA;
        if (health > maxHealth)
            health = maxHealth;
    }

    protected void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "heals" && health < maxHealth)
        {
            audioSource.PlayOneShot(eatingMeatSounds[Random.Range(0, eatingMeatSounds.Length)]);
            Heals(30);
            other.gameObject.SetActive(false);
        }
    }

    protected void PlayAttackSound()
    {
        if (currentWeapon == 0)
            audioSource.PlayOneShot(swordAttackSounds[Random.Range(0, swordAttackSounds.Length)]);
        else if (currentWeapon == 1)
            audioSource.PlayOneShot(maceAttackSounds[Random.Range(0, maceAttackSounds.Length)]);
        else if (currentWeapon == 2)
            audioSource.PlayOneShot(axeAttackSounds[Random.Range(0, axeAttackSounds.Length)]);
    }
}
