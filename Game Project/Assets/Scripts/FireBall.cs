using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float BallSpeed;
    public float maxDistance;
    private float distance;
    public int damage;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * BallSpeed);

        distance += Time.deltaTime;

        if(distance >= maxDistance)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            other.GetComponent<EnemyAI>().TakeDamage(damage);
        }

        if (other.gameObject.tag == "Boss")
        {
            other.GetComponent<BossAI>().TakeDamage(damage);
        }

        else if (other.gameObject.tag == "Wall")
        {
            GetComponent<ParticleSystem>().Stop();
            GetComponent<Collider>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
