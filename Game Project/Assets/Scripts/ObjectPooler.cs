using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    public GameObject[] goblinPrefabs, golemPrefabs, skeletonKnightPrefabs;

    public int enemyTypes = 3;

    public enum PooledObjectType
    {
        Goblin,
        Golem,
        SkeletonKnight,
    }

    private struct PooledObject
    {
        public GameObject m_GameObject;
        public int m_PrefabIndex;
        public PooledObjectController m_Controller;
    }

    private List<PooledObject> goblinPool = new List<PooledObject>();
    private List<PooledObject> golemPool = new List<PooledObject>();
    private List<PooledObject> skeletonKnightPool = new List<PooledObject>();

    public GameObject SpawnPooledObject(PooledObjectType objectType, int prefabIndex, Vector3 spawnPoint, Vector3 spawnRotation)
    {
        List<PooledObject> objectPool = null;
        GameObject[] objectPrefabs = null;
        Transform objectParent = null;
        switch (objectType)
        {
            case PooledObjectType.Goblin:
                objectPool = goblinPool;
                objectPrefabs = goblinPrefabs;
                objectParent = transform.Find("GoblinPool");
                break;
            case PooledObjectType.Golem:
                objectPool = golemPool;
                objectPrefabs = golemPrefabs;
                objectParent = transform.Find("GolemPool");
                break;
            case PooledObjectType.SkeletonKnight:
                objectPool = skeletonKnightPool;
                objectPrefabs = skeletonKnightPrefabs;
                objectParent = transform.Find("SkeletonKnightPool");
                break;
        }

        PooledObject pooledObject;

        //Search through the object pool for an object of the correct type that is free
        //If one is found set it to be the spawned object then return out of the function
        for (int i = 0; i < objectPool.Count; i++)
        {
            if (objectPool[i].m_PrefabIndex == prefabIndex && objectPool[i].m_GameObject.activeInHierarchy == false)
            {
                pooledObject = objectPool[i];
                pooledObject.m_GameObject.transform.position = spawnPoint;
                pooledObject.m_GameObject.transform.rotation = Quaternion.Euler(spawnRotation);
                pooledObject.m_GameObject.SetActive(true);
                if (pooledObject.m_Controller != null)
                    pooledObject.m_Controller.OnSpawnObject();
                return pooledObject.m_GameObject;
            }
        }

        //If there is no suitable object in the pool instantiate a new object and add it to the pool
        GameObject gameObject = (Instantiate(objectPrefabs[prefabIndex], spawnPoint, Quaternion.Euler(spawnRotation), objectParent));

        pooledObject = new PooledObject
        {
            m_GameObject = gameObject,
            m_PrefabIndex = prefabIndex,
            m_Controller = gameObject.GetComponent<PooledObjectController>()
        };

        objectPool.Add(pooledObject);
        if (pooledObject.m_Controller != null)
            pooledObject.m_Controller.OnSpawnObject();
        return pooledObject.m_GameObject;
    }
}
