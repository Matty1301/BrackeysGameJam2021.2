using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        volumeSlider.value = PublicVariables.volume;
    }

    // Update is called once per frame
    void Update()
    {
        audioMixer.SetFloat("Volume", PublicVariables.volume);
    }
}
