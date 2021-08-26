using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicVariables : MonoBehaviour
{
    public static int volume;
    public static int quality;
    public static int Jewels;

    public void setVolume(int input)
    {
        volume = input;
    }
}
