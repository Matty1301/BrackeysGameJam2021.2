using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public void setVolume(float input)
    {
        PublicVariables.volume = (int)input;
    }

    public void setQuality(int quality)
    {
        PublicVariables.quality = quality;
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetFullScreen(bool FullScreen)
    {
        Screen.fullScreen = FullScreen;
    }
}
