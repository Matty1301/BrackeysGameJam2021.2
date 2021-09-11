using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public static UnityEngine.Events.UnityAction playerSpawned;

    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] rightRooms;
    public GameObject[] leftRooms;
    public GameObject closedRoom;
    public GameObject[] allRooms;

    public List<GameObject> rooms;

    public float waitTime;
    private bool spawnedBoss;
    public GameObject bossPrefab;
    public GameObject Win;
    private GameObject Boss;

    private ObjectPooler objectPooler;

    public List<GameObject> Collectables;

    public List<GameObject> Characters;

    private void Awake()
    {
        objectPooler = FindObjectOfType<ObjectPooler>();

        Characters[PublicVariables.character].SetActive(true);
        playerSpawned?.Invoke();
    }

    private void Update()
    {
        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }

        else if (waitTime <= 0 && spawnedBoss == false)
        {
            FindObjectOfType<NavMeshBuilder>().BuildNavMesh();

            for (int i = 1; i < rooms.Count; i++)
            {
                rooms[i].GetComponent<RoomScript>().spawnCollectables();

                if (i == rooms.Count - 1)
                {
                    Boss = Instantiate(bossPrefab, rooms[i].transform.position, Quaternion.identity).gameObject;
                    rooms[i].GetComponent<RoomScript>().Enemies.Add(Boss);
                    spawnedBoss = true;
                }
                else
                {
                    rooms[i].GetComponent<RoomScript>().SpawnEnemies();
                }
            }
        }

        else if (spawnedBoss)
        {
            if (!Boss.activeInHierarchy)
            {
                Win.SetActive(true);

                for (int i = 1; i < rooms.Count; i++)
                {
                    rooms[i].GetComponent<RoomScript>().disableEnemies();
                }
            }
        }
    }

    public void AddRoom(GameObject CurrentRoom)
    {
        rooms.Add(CurrentRoom);
    }
}