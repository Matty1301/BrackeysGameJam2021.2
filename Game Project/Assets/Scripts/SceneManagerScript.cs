using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void QuitGame()
    {
        Debug.Log("Quit!!");
        Application.Quit();
    }

    public void setTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}
