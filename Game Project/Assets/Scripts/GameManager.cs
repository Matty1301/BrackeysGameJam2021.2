using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        Pregame,
        Running,
        Paused,
        Postgame
    }
    public GameState currentGameState { get; private set; }

    public void TogglePause()
    {
        if (currentGameState == GameState.Running)
            ChangeGameState(GameState.Paused);
        else if (currentGameState == GameState.Paused)
            ChangeGameState(GameState.Running);
    }

    private void ChangeGameState(GameState newGameState)
    {
        if (newGameState == GameState.Paused)
            Time.timeScale = 0;
        else
            Time.timeScale = 1.0f;

        currentGameState = newGameState;
    }

    private void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void StartGame()
    {
        ChangeGameState(GameState.Running);
        LoadScene("Main");
    }

    public void GameOver()
    {
        ChangeGameState(GameState.Postgame);
        LoadScene("GameOver");
    }

    public void ReturnToMainMenu()
    {
        ChangeGameState(GameState.Pregame);
        LoadScene("MainMenu");
    }
}
