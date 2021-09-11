using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] pickupSounds;

    public int JewelValue = 0;
    public float SpeedValue = 0;
    private float initialSpeed;
    public MeshRenderer MushroomMeshRenderer;

    public bool jewel;
    public bool speedBoost;

    private Controller playerController;

    private void Start()
    {
        audioSource = GameObject.Find("SoundEffectsSource").GetComponent<AudioSource>();
        playerController = FindObjectOfType<Controller>();
        //initialSpeed = playerController.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (jewel)
            {
                PlayPickupSound();
                PublicVariables.Jewels += JewelValue;
                gameObject.SetActive(false);
            }
            else if (speedBoost)
            {
                speedBoost = false;
                PlayPickupSound();
                playerController.speed = playerController.speed * 2;
                MushroomMeshRenderer.enabled = false;
                //StartCoroutine(resetSpeed(SpeedValue));
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator resetSpeed(float delay)
    {
        yield return new WaitForSeconds(delay);

        playerController.speed /= 2;

        gameObject.SetActive(false);
    }

    private void PlayPickupSound()
    {
        audioSource.PlayOneShot(pickupSounds[Random.Range(0, pickupSounds.Length)]);
    }
}
