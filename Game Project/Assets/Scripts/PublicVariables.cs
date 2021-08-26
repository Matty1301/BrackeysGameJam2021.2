using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicVariables : MonoBehaviour
{
    public static int volume;

    public void setVolume(int input)
    {
        volume = input;
    }
}
