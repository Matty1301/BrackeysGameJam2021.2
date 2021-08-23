using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;


    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerIsInSightRange, playerIsInAttackRange;

    public GameObject Bullet;
    public Transform ShootPoint;
    public float health;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerIsInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerIsInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerIsInSightRange && !playerIsInAttackRange) Patrolling();
        if (playerIsInSightRange && !playerIsInAttackRange) ChasePlayer();
        if (playerIsInSightRange && playerIsInAttackRange) AttackPlayer();
    }

    private void SearchWalkPoint()
    {
        float RandomZ = Random.Range(-walkPointRange, walkPointRange);
        float RandomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + RandomX, transform.position.y, transform.position.z + RandomZ);

        //If there are no obstructions between the enemy and the new walk point, then set walk point to true
        if (NavMesh.Raycast(transform.position, walkPoint, out NavMeshHit hit, NavMesh.AllAreas) == false)
        {
            walkPointSet = true;
        }
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
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
            ObjectPooler.Instance.SpawnPooledObject(ObjectPooler.PooledObjectType.Bullet, 0, ShootPoint.position, transform.rotation.eulerAngles);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), .5f);
        }
    }


    private void DestroyEnemy()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            TakeDamage(10);
        }
    }
}

