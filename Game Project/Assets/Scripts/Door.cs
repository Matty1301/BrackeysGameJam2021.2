using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] AudioClip doorLocking, doorUnlocking, doorClosing, doorOpening;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void DoorClosing()
    {
        audioSource.PlayOneShot(doorClosing);
    }

    private void DoorLocking()
    {
        audioSource.PlayOneShot(doorLocking);
    }

    private void DoorUnlocking()
    {
        audioSource.PlayOneShot(doorUnlocking);
    }

    private void DoorOpening()
    {
        audioSource.PlayOneShot(doorOpening);
    }
}
