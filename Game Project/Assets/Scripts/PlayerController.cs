using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private Camera camera;
    private float speed = 500;
    private Vector3 mousePosWorld;
    public Transform shootPoint;
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public float health = 100f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        camera = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        MovePlayer();
        RotatePlayer();
        if (Input.GetMouseButton(0) && !alreadyAttacked)
            Attack();
    }

    private void MovePlayer()
    {
        rigidbody.velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rigidbody.velocity += new Vector3(0, 0, speed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.velocity += new Vector3(-speed * Time.fixedDeltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            rigidbody.velocity += new Vector3(0, 0, -speed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.velocity += new Vector3(speed * Time.fixedDeltaTime, 0, 0);
        }
    }

    private void RotatePlayer()
    {
        mousePosWorld = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.transform.position.y));

        if (Mathf.Abs((mousePosWorld - transform.position).sqrMagnitude) < 0.8f)
            rigidbody.angularVelocity = Vector3.zero;   //If the mouse is too close to the player, don't rotate
        else
            transform.LookAt(new Vector3(mousePosWorld.x, transform.position.y, mousePosWorld.z));
    }

    /*
    private IEnumerator Shoot()
    {
        alreadyAttacked = true;
        ObjectPooler.Instance.SpawnPooledObject(ObjectPooler.PooledObjectType.Bullet, 0, shootPoint.position, transform.rotation.eulerAngles);
        yield return new WaitForSeconds(rateOfFire);
        alreadyAttacked = false;
    }
    */

    private void Attack()
    {
        ObjectPooler.Instance.SpawnPooledObject(ObjectPooler.PooledObjectType.Bullet, 0, shootPoint.position, transform.rotation.eulerAngles);

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
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
            Debug.Log("Game over");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            TakeDamage(10);
        }
    }
}
