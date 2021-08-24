using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawWeapon : MonoBehaviour
{
    private void Start()
    {
        GetComponent<RPGCharacterAnimsFREE.RPGCharacterInputController>().Invoke("DrawWeapon", 2);
    }
}
