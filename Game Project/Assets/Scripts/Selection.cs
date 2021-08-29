using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public GameObject[] characters;

    public void PreviousCharacter()
    {
        characters[PublicVariables.character].SetActive(false);
        PublicVariables.character = (PublicVariables.character + 1) % characters.Length;
        characters[PublicVariables.character].SetActive(true);
    }

    public void NextCharacter()
    {
        characters[PublicVariables.character].SetActive(false);
        PublicVariables.character--;

        if(PublicVariables.character < 0)
        {
            PublicVariables.character += characters.Length;
        }

        characters[PublicVariables.character].SetActive(true);
    }

}
