using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsPlayer;

    private Vector3 startPos;
    public Vector3 walkPoint;
    private bool walkPointSet = true;
    public float walkPointRange;

    public float timeBetweenAttacks;
    public bool alreadyAttacked;
    public Transform attackPoint;
    private float attackVolume = 1;
    private Collider[] targets;

    public float sightRange, attackRange;
    public bool playerIsInSightRange, playerIsInAttackRange;

    public int maxHealth;
    [HideInInspector] public int health;
    [SerializeField] GameObject ragdollPrefab;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
        walkPoint = startPos;
    }

    private void Update()
    {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        playerIsInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerIsInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerIsInSightRange && !playerIsInAttackRange) Patrolling();
        if (playerIsInSightRange && !playerIsInAttackRange) ChasePlayer();
        if (playerIsInSightRange && playerIsInAttackRange) AttackPlayer();
    }

    private void SearchWalkPoint()
    {
        if (walkPointSet == false)
        {
            float RandomZ = Random.Range(-1, 2);
            float RandomX = Random.Range(-1, 2);

            walkPoint = new Vector3(startPos.x + (walkPointRange * RandomX), transform.position.y, startPos.z + (walkPointRange * RandomZ));

            //If there are no obstructions between the enemy and the new walk point, then set walk point to true
            if (NavMesh.Raycast(transform.position, walkPoint, out NavMeshHit hit, NavMesh.AllAreas) == false)
            {
                agent.SetDestination(walkPoint);
                walkPointSet = true;
            }
            else
                SearchWalkPoint();
        }
    }

    private void Patrolling()
    {
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f && walkPointSet)
        {
            walkPointSet = false;
            Invoke("SearchWalkPoint", 5);
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("Attack");
            alreadyAttacked = true;
            Invoke("RegisterHits", 0.8f);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void RegisterHits()
    {
        targets = Physics.OverlapSphere(attackPoint.position, attackVolume, 1 << LayerMask.NameToLayer("Player"));
        foreach (Collider target in targets)
        {
            if (target.GetComponent<PlayerController>() != null)
                target.GetComponent<PlayerController>().TakeDamage(10);
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
        gameObject.SetActive(false);
        GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        Vector3 directionFromHitPoint = Vector3.zero;
        if (FindObjectOfType<PlayerController>())
            directionFromHitPoint = ragdoll.transform.position - FindObjectOfType<PlayerController>().attackPoint.position;
        int forceMultiplier = 100;
        ragdoll.GetComponent<Rigidbody>().AddForce(directionFromHitPoint * forceMultiplier, ForceMode.Impulse);
    }
}

