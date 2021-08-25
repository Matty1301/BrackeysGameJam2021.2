using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    private CharacterController enemyAI;
    private Slider healthBarFill;

    private void Awake()
    {
        enemyAI = transform.GetComponentInParent<CharacterController>();
        healthBarFill = GetComponent<Slider>();
    }

    void Start()
    {
        healthBarFill.value = 1;
    }

    void Update()
    {
        if (enemyAI != null)
        {
            healthBarFill.value = (float)enemyAI.health / enemyAI.maxHealth;
        }
        transform.parent.LookAt(Camera.main.transform);
    }
}
