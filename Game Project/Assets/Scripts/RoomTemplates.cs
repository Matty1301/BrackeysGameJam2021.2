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
    public List<GameObject> Enemies;

    public float waitTime;
    private bool spawnedBoss;
    public GameObject bossPrefab;
    public GameObject enemyPrefab;
    public GameObject Win;
    private GameObject Boss;

    private void Update()
    {
        if(waitTime <= 0 && spawnedBoss == false)
        {
            FindObjectOfType<NavMeshBuilder>().BuildNavMesh();

            for (int i = 1; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    Boss = Instantiate(bossPrefab, rooms[i].transform.position, Quaternion.identity).gameObject;
                    spawnedBoss = true;
                }
                else
                {
                    int x = 0;
                    int randomX = Random.RandomRange(1, 2);
                    while (x < 1)
                    {
                        x++;
                        Enemies.Add(Instantiate(enemyPrefab, rooms[i].transform.position, Quaternion.identity));
                    }
                }
            }
        }

        else
        {
            waitTime -= Time.deltaTime;
        }
        if (spawnedBoss)
        {
            if (!Boss.active)
            {
                Win.SetActive(true);
                int EnnemiesNumber = Enemies.Count;
                for (int i = 0; i < EnnemiesNumber; i++)
                {
                    Enemies[i].SetActive(false);
                }
            }
        }
    }
}