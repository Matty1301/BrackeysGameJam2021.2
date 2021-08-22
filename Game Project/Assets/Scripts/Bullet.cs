using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, PooledObjectController
{
    private Rigidbody rigidbody;
    private float speed = 20;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void OnSpawnObject()
    {
        StartCoroutine(Lifetime());
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void Update()
    {
        //If the game object goes outside the map, disable it
        if (transform.position.y < 0)
            gameObject.SetActive(false);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}
