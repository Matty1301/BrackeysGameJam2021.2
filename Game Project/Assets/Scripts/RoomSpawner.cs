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
        Destroy(gameObject, waitTime);
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("Spawn", 0.1f);
    }


    void Spawn()
    {
        if(spawned == false)
        {

            if (openingDirection == 1)
            {
                //spawn a room with a BOTTOM door
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation).gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            }

            else if (openingDirection == 2)
            {
                //spawn a room with a TOP door
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation).gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            }

            else if (openingDirection == 3)
            {
                //spawn a room with a LEFT door
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation).gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            }

            else if (openingDirection == 4)
            {
                //spawn a room with a RIGHT door
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation).gameObject.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
            }

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
