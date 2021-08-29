using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public RoomTemplates roomTemplate;

    public List<GameObject> Enemies;
    public List<GameObject> Doors;
    public bool doorsOpen;
    public List<GameObject> Collectables;
    private ObjectPooler objectPooler;

    [SerializeField] private int EnemyNum;
    private bool spawnedEnemies = false;

    public SetWeaponMelee setWeaponMelee;
    public SetWeaponWizard setWeaponWizard;
    public SetWeaponArcher setWeaponArcher;


    void Start()
    {
        roomTemplate = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        roomTemplate.AddRoom(gameObject);
        Collectables = roomTemplate.Collectables;
        objectPooler = FindObjectOfType<ObjectPooler>();

        if (PublicVariables.character == 0)
        {
            setWeaponMelee = GameObject.FindGameObjectWithTag("Player").GetComponent<SetWeaponMelee>();
        }

        if (PublicVariables.character == 1)
        {
            setWeaponWizard = GameObject.FindGameObjectWithTag("Player").GetComponent<SetWeaponWizard>();
        }

        if (PublicVariables.character == 2)
        {
            setWeaponArcher = GameObject.FindGameObjectWithTag("Player").GetComponent<SetWeaponArcher>();
        }

        openDoors();

        Invoke("EnableDoorAudio", 2);
    }

    private void EnableDoorAudio()
    {
        foreach (GameObject door in Doors)
        {
            door.GetComponent<AudioSource>().enabled = true;
        }
    }

    void Update()
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i].activeInHierarchy == false)
                Enemies.Remove(Enemies[i]);
        }

        EnemyNum = Enemies.Count;

        if(EnemyNum == 0 && spawnedEnemies && !doorsOpen)
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

    public void spawnCollectables()
    {
        for (int i = 0; i < Random.Range(2, 4); i++)
        {
            Instantiate(Collectables[Random.Range(0, Collectables.Count - 1)], new Vector3(Random.Range(-11, 11) + transform.position.x, transform.position.y + 0.5f, Random.Range(-11, 11) + transform.position.z), Quaternion.identity);
        }
    }


    public void disableEnemies()
    {
        foreach (GameObject enemy in Enemies)
        {
            enemy.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && EnemyNum > 0 && doorsOpen)
        {
            swapPlayerWeapon();
            closeDoors();
        }
        if (other.gameObject.tag == "Enemy")
        {
            EnemyNum++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyNum--;
        }

        if (other.gameObject.tag == "Player" && !doorsOpen)
        {
            openDoors();
        }
    }

    public void closeDoors()
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            Animation animation = Doors[i].GetComponent<Animation>();
            animation.clip = animation.GetClip("DoorClosing");
            animation.Play();
        }
        doorsOpen = false;
    }

    public void openDoors()
    {
        for (int i = 0; i < Doors.Count; i++)
        {
            Animation animation = Doors[i].GetComponent<Animation>();
            animation.clip = animation.GetClip("DoorOpening");
            animation.Play();
        }
        doorsOpen = true;
    }

    private void swapPlayerWeapon()
    {
        if (PublicVariables.character == 0)
        {
            if (setWeaponMelee != null)
            {
                int RandomWeapon = Random.Range(0, setWeaponMelee.Weapons.Count);
                Debug.Log("Weapon swapped : " + RandomWeapon);
                setWeaponMelee.ChangeWeapon(RandomWeapon);
            }
        }

        if (PublicVariables.character == 1)
        {
            if (setWeaponWizard != null)
            {
                int RandomWeapon = Random.Range(0, setWeaponWizard.Weapons.Count);
                Debug.Log("Weapon swapped : " + RandomWeapon);
                setWeaponWizard.ChangeWeapon(RandomWeapon);
            }
        }

        if (PublicVariables.character == 2)
        {
            if (setWeaponArcher != null)
            {
                int RandomWeapon = Random.Range(0, setWeaponArcher.Weapons.Count);
                Debug.Log("Weapon swapped : " + RandomWeapon);
                setWeaponArcher.ChangeWeapon(RandomWeapon);
            }
        }
    }
}
