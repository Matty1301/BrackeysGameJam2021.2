using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] rightRooms;
    public GameObject[] leftRooms;
    public GameObject closedRoom;

    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnedBoss;
    public GameObject boss;
    public GameObject enemy;

    private void Update()
    {
        if(waitTime <= 0 && spawnedBoss == false)
        {
            FindObjectOfType<NavMeshBuilder>().BuildNavMesh();

            for (int i = 1; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                    spawnedBoss = true;
                }
                else
                    Instantiate(enemy, rooms[i].transform.position, Quaternion.identity);
            }
        }

        else
        {
            waitTime -= Time.deltaTime;
        }
    }
}
