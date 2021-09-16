using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterController : MonoBehaviour
{
    protected Animator animator;
    public NavMeshAgent agent;
    protected AudioSource audioSource;

    [SerializeField] protected AudioClip[] hitSounds;

    public Transform player;
    public LayerMask whatIsPlayer;

    protected Vector3 startPos;

    [SerializeField] private float timeBeforeRegisterHits;
    public float timeBetweenAttacks;
    public bool alreadyAttacked;
    public Transform attackPoint;
    [SerializeField] protected float attackVolume;
    protected Collider[] targets;

    public int weaponDamage;
    public float sightRange, attackRange;
    public bool playerIsInSightRange, playerIsInAttackRange;

    public int maxHealth;
    [HideInInspector] public int health;
    [SerializeField] protected GameObject ragdollPrefab;

    protected void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        player = FindObjectOfType<Controller>().transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GameObject.Find("SoundEffectsSource").GetComponent<AudioSource>();
        startPos = transform.position;
        health = maxHealth;
    }

    protected virtual void Update()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        playerIsInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerIsInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //if (!playerIsInSightRange && !playerIsInAttackRange) ;
        if (playerIsInSightRange && !playerIsInAttackRange) ChasePlayer();
        if (playerIsInSightRange && playerIsInAttackRange) AttackPlayer();
    }

    protected void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    protected void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            animator.ResetTrigger("Hit");
            animator.SetTrigger("Attack");
            alreadyAttacked = true;
            Invoke("RegisterHits", timeBeforeRegisterHits);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    protected void RegisterHits()
    {
        if (gameObject.activeInHierarchy)
        {
            targets = Physics.OverlapSphere(attackPoint.position, attackVolume, 1 << LayerMask.NameToLayer("Player"));
            foreach (Collider target in targets)
            {
                if (target.GetComponent<Controller>() != null)
                    target.GetComponent<Controller>().TakeDamage(weaponDamage);
            }
        }
    }

    protected void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Hit");
        audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);

        health -= damage;

        if (health <= 0)
        {
            Death();
        }
        else
        {
            ChasePlayer();
        }
    }

    protected virtual void Death()
    {
        gameObject.SetActive(false);
        GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        Vector3 directionFromHitPoint = Vector3.zero;
        if (FindObjectOfType<Controller>())
            directionFromHitPoint = ragdoll.transform.position - FindObjectOfType<Controller>().attackPoint.position;
        int forceMultiplier = 100;
        ragdoll.GetComponent<Rigidbody>().AddForce(directionFromHitPoint * forceMultiplier, ForceMode.Impulse);
    }
}