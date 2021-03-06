using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOld : MonoBehaviour
{
    public Transform attackPoint;
    private float attackVolume = 1;
    private Collider[] targets;
    public GameObject Win, Lose;


    public int maxHealth;
    [HideInInspector] public int health;

    [SerializeField] GameObject ragdollPrefab;

    private void Start()
    {
        GetComponent<RPGCharacterAnimsFREE.RPGCharacterInputController>().DrawWeapon();
        health = maxHealth;
    }

    private void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo,
                Mathf.Infinity, 1 << LayerMask.NameToLayer("Walkable"));
            transform.LookAt(new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z));
        }
    }

    public void RegisterHits()
    {
        if (gameObject.activeInHierarchy)
        {
            targets = Physics.OverlapSphere(attackPoint.position, attackVolume, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (Collider target in targets)
            {
                if (target.GetComponent<CharacterController>() != null)
                    target.GetComponent<CharacterController>().TakeDamage(25);
            }
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
        Lose.SetActive(true);
        gameObject.SetActive(false);
        GameObject ragdoll = Instantiate(ragdollPrefab, transform.position, transform.rotation);
    }

    private void Heals(int healthA)
    {
        health += healthA;
        if (health > maxHealth)
            health = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "heals")
        {
            Heals(30);
            other.gameObject.SetActive(false);
        }
    }
}
