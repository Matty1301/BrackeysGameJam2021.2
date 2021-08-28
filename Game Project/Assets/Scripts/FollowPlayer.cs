using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject knight, wizard;

    private void Update()
    {
        if (knight.activeInHierarchy)
            transform.position = knight.transform.position;
        else if (wizard.activeInHierarchy)
            transform.position = wizard.transform.position;
    }
}
