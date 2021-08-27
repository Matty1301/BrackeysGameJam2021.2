using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{

    public int JewelValue = 0;
    public float SpeedValue = 0;
    private float initialSpeed;
    public MeshRenderer MushroomMeshRenderer;

    public bool jewel;
    public bool speedBoost;

    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        initialSpeed = playerController.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (jewel)
            {
                PublicVariables.Jewels += JewelValue;
                gameObject.SetActive(false);
            }
            else if (speedBoost)
            {
                playerController.speed = initialSpeed * 2;
                StartCoroutine(resetSpeed(SpeedValue));
            }
        }
    }

    private IEnumerator resetSpeed(float delay)
    {
        MushroomMeshRenderer.enabled = false;

        yield return new WaitForSeconds(delay);

        playerController.speed = initialSpeed;

        gameObject.SetActive(false);
    }
}
