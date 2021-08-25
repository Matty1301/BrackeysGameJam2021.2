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

    private ObjectPooler objectPooler;

    private void Awake()
    {
        objectPooler = FindObjectOfType<ObjectPooler>();
    }

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
                    for (int enemiesToSpawn = 0; enemiesToSpawn < Random.Range(2, 6); enemiesToSpawn++)
                    {
                        Enemies.Add(objectPooler.SpawnPooledObject(ObjectPooler.PooledObjectType.Enemy, Random.Range(0, objectPooler.enemyPrefabs.Length),
                            rooms[i].transform.position, Vector3.zero));
                    }
                }
            }
        }

        else if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        if (spawnedBoss)
        {
            if (!Boss.activeInHierarchy)
            {
                Win.SetActive(true);
                int enemiesNumber = Enemies.Count;
                for (int i = 0; i < enemiesNumber; i++)
                {
                    Enemies[i].SetActive(false);
                }
            }
        }
    }
}