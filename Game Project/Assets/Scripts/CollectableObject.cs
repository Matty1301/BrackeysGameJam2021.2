using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{

    public int JewelValue = 0;
    public int SpeedValue = 0;
    private float initialSpeed;

    public bool jewel;
    public bool speedBoost;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        initialSpeed = playerController.speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(jewel)
            {
                PublicVariables.Jewels += JewelValue;
                gameObject.SetActive(false);
            }
            else if(speedBoost)
            {
                playerController.speed = initialSpeed * 2;
                Invoke("resetSpeed()", SpeedValue);
            }
        }
    }

    public void resetSpeed()
    {
        playerController.speed = initialSpeed;
        Debug.Log("Reset!!");
        gameObject.SetActive(false);
    }
}
