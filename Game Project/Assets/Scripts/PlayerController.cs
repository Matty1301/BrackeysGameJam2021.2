using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform attackPoint;
    private float attackVolume = 1;
    Collider[] targets;

    public void RegisterHits()
    {
        targets = Physics.OverlapSphere(attackPoint.position, attackVolume, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (Collider target in targets)
        {
            Debug.Log(target.gameObject.name);

            if (target.GetComponent<EnemyAI>() != null)
                target.GetComponent<EnemyAI>().TakeDamage(20);
        }
    }
}
