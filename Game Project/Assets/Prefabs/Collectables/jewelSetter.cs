using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jewelSetter : MonoBehaviour
{
    public Text Jewels;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Jewels.text = PublicVariables.Jewels.ToString();
    }
}
