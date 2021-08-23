using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBuilder : MonoBehaviour
{
    public void BuildNavMesh()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
