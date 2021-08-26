using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public RoomTemplates roomTemplate;

    public List<GameObject> Enemies;
    public List<GameObject> Doors;
    private ObjectPooler objectPooler;

    [SerializeField] private int EnemyNum;
    private bool spawnedEnemies = false;

    void Start()
    {
        roomTemplate = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        roomTemplate.AddRoom(gameObject);
        objectPooler = FindObjectOfType<ObjectPooler>();

        openDoors();
    }

    void Update()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].activeInHierarchy == false)
                Enemies.Remove(Enemies[i]);
        }

        EnemyNum = Enemies.Count;

        if(EnemyNum == 0 && spawnedEnemies)
        {
            openDoors();
        }
    }

    public void SpawnEnemies()
    {
        for (int enemiesToSpawn = 0; enemiesToSpawn < Random.Range(2, 6); enemiesToSpawn++)
        {
            Enemies.Add(objectPooler.SpawnPooledObject(ObjectPooler.PooledObjectType.Enemy, Random.Range(0, objectPooler.enemyPrefabs.Length),
                transform.position, Vector3.zero));
        }
        spawnedEnemies = true;
    }

    public void disableEnemies()
    {
        for (int i = 0; i < EnemyNum; i++)
        {
            Enemies[i].SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            closeDoors();
        }
        if (other.gameObject.tag == "Enemy")
        {
            EnemyNum++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //openDoors();
        }
        if (other.gameObject.tag == "Enemy")
        {
            EnemyNum--;
        }
    }

    public void closeDoors()
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            Doors[i].SetActive(true);
        }
    }

    public void openDoors()
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            Doors[i].SetActive(false);
        }
    }
}