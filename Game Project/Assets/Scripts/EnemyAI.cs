using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : CharacterController
{
    private Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    private void Start()
    {
        SearchWalkPoint();
    }

    protected override void Update()
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
        if (walkPointSet == false && gameObject.activeInHierarchy)
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
        }
    }

    private void Patrolling()
    {
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if ((distanceToWalkPoint.magnitude < 1f && walkPointSet) || !walkPointSet)
        {
            walkPointSet = false;
            Invoke("SearchWalkPoint", 5);
        }
    }
}

