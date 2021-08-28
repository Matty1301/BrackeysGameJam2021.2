using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float FireBallForce;
    public float maxDistance;
    private float distance;

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * FireBallForce);

        distance += Time.deltaTime;
        if(maxDistance < distance)
        {
            Destroy(this.gameObject);
        }
    }
}
