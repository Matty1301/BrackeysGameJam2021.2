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

    private PlayerController playerController;

    private void Start()
    {
        audioSource = GameObject.Find("SoundEffectsSource").GetComponent<AudioSource>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        initialSpeed = playerController.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayPickupSound();
            if (jewel)
            {
                PublicVariables.Jewels += JewelValue;
                gameObject.SetActive(false);
            }
            else if (speedBoost)
            {
                Debug.Log("Double speed");
                playerController.speed = playerController.speed * 2;
                StartCoroutine(resetSpeed(SpeedValue));
            }
        }
    }

    private IEnumerator resetSpeed(float delay)
    {
        MushroomMeshRenderer.enabled = false;

        yield return new WaitForSeconds(delay);

        playerController.speed = playerController.GetComponent<SetWeapon>().speed[playerController.currentWeapon];

        gameObject.SetActive(false);
    }

    private void PlayPickupSound()
    {
        audioSource.PlayOneShot(pickupSounds[Random.Range(0, pickupSounds.Length)]);
    }
}
