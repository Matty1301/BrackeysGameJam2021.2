using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    private RoomTemplates templates;
    private int rand;
    private bool spawned = false;

    public float waitTime = 4f;

    private void Awake()
    {
        Invoke("Disable", waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }


    void Spawn()
    {
        if(spawned == false)
        {
            //spawn a room with the door facing the previous room
            rand = Random.Range(0, templates.allRooms.Length);
            Instantiate(templates.allRooms[rand], transform.position, transform.rotation)
                .gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);

            spawned = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("RoomSpawnPoint"))
        {
            if(other.GetComponent<RoomSpawner>() && other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity).gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
