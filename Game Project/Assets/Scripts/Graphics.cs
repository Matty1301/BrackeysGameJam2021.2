using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graphics : MonoBehaviour
{
    public Dropdown graphics;
    
    // Start is called before the first frame update
    void Start()
    {
        graphics.value = PublicVariables.quality;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
