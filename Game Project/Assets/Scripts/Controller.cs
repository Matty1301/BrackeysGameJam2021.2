using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    protected Animator animator;
    public float speed;

    [HideInInspector] public int currentWeapon;

    public Transform attackPoint;
    public GameObject Win, Lose;

    public int maxHealth;
    [HideInInspector] public int health;

    [SerializeField] protected GameObject ragdollPrefab;

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
}
