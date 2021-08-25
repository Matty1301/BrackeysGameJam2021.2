using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform attackPoint;
    private float attackVolume = 1;
    private Collider[] targets;

    private int health = 100;

    [SerializeField] GameObject ragdollPrefab;

    private void Start()
    {
        GetComponent<RPGCharacterAnimsFREE.RPGCharacterInputController>().DrawWeapon();
    }

    public void RegisterHits()
    {
        targets = Physics.OverlapSphere(attackPoint.position, attackVolume, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (Collider target in targets)
        {
            if (target.GetComponent<EnemyAI>() != null)
                target.GetComponent<EnemyAI>().TakeDamage(20);
        }
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
        gameObject.SetActive(false);
        GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
    }
}
