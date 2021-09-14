using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage;
    public float torque;
    public Rigidbody rb;

    private bool DidHit;

    public void Fly(Vector3 force)
    {
        rb.isKinematic = false;
        rb.AddForce(force, ForceMode.Impulse);
        rb.AddTorque(transform.right * torque);
        transform.SetParent(null);
        GetComponent<Collider>().enabled = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (DidHit) return;
        DidHit = true;

        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyAI>().TakeDamage(damage);
        }

        if (other.gameObject.tag == "Boss")
        {
            other.gameObject.GetComponent<BossAI>().TakeDamage(damage);
        }

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.SetParent(other.transform);
        GetComponent<Collider>().enabled = false;
        if (GetComponentInChildren<ParticleSystem>() != null)
            GetComponentInChildren<ParticleSystem>().Stop();
    }
}
