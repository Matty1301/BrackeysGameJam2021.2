using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selection : MonoBehaviour
{
    public GameObject[] characters;
    private Animator[] animators;

    private void Start()
    {
        animators = new Animator[characters.Length];

        for (int i = 0; i < characters.Length; i++)
        {
            animators[i] = characters[i].GetComponent<Animator>();
        }
    }

    private void OnEnable()
    {
        PublicVariables.character = 0;
    }

    public void NextCharacter()
    {
        characters[PublicVariables.character].SetActive(false);
        PublicVariables.character = (PublicVariables.character + 1) % characters.Length;
        characters[PublicVariables.character].SetActive(true);
    }

    public void PreviousCharacter()
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
