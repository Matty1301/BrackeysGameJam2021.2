using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitRegistration : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    private float attackVolume = 1;
    Collider[] targets;

    public void RegisterHits()
    {
        targets = Physics.OverlapSphere(attackPoint.position, attackVolume, LayerMask.NameToLayer("Enemy"));
        foreach (Collider target in targets)
        {
            if (target.GetComponent<EnemyAI>() != null)
                target.GetComponent<EnemyAI>().TakeDamage(20);
        }
    }
}
