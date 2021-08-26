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
        int enemyType = Random.Range(0, objectPooler.enemyTypes + 1);
        switch (enemyType)
        {
            case 0:
                for (int goblinsToSpawn = 0; goblinsToSpawn < Random.Range(2, 6); goblinsToSpawn++)
                {
                    Enemies.Add(objectPooler.SpawnPooledObject(ObjectPooler.PooledObjectType.Goblin,
                        Random.Range(0, objectPooler.goblinPrefabs.Length), transform.position, Vector3.zero));
                }
                break;
            case 1:
                for (int golemsToSpawn = 0; golemsToSpawn < Random.Range(1, 3); golemsToSpawn++)
                {
                    Enemies.Add(objectPooler.SpawnPooledObject(ObjectPooler.PooledObjectType.Golem,
                        Random.Range(0, objectPooler.golemPrefabs.Length), transform.position, Vector3.zero));
                }
                break;
            case 2:
                for (int skeletonKnightsToSpawn = 0; skeletonKnightsToSpawn < Random.Range(3, 8); skeletonKnightsToSpawn++)
                {
                    Enemies.Add(objectPooler.SpawnPooledObject(ObjectPooler.PooledObjectType.SkeletonKnight,
                        Random.Range(0, objectPooler.skeletonKnightPrefabs.Length), transform.position, Vector3.zero));
                }
                break;
            case 3:
                for (int goblinsToSpawn = 0; goblinsToSpawn < Random.Range(1, 3); goblinsToSpawn++)
                {
                    Enemies.Add(objectPooler.SpawnPooledObject(ObjectPooler.PooledObjectType.Goblin,
                        Random.Range(0, objectPooler.goblinPrefabs.Length), transform.position, Vector3.zero));
                }
                for (int golemsToSpawn = 0; golemsToSpawn < Random.Range(1, 2); golemsToSpawn++)
                {
                    Enemies.Add(objectPooler.SpawnPooledObject(ObjectPooler.PooledObjectType.Golem,
                        Random.Range(0, objectPooler.golemPrefabs.Length), transform.position, Vector3.zero));
                }
                for (int skeletonKnightsToSpawn = 0; skeletonKnightsToSpawn < Random.Range(2, 4); skeletonKnightsToSpawn++)
                {
                    Enemies.Add(objectPooler.SpawnPooledObject(ObjectPooler.PooledObjectType.SkeletonKnight,
                        Random.Range(0, objectPooler.skeletonKnightPrefabs.Length), transform.position, Vector3.zero));
                }
                break;
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
