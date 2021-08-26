using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{

    public int JewelValue = PublicVariables.Jewels;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PublicVariables.Jewels += JewelValue;
            Debug.Log("Enter!!");
            gameObject.SetActive(false);
        }
    }
}
