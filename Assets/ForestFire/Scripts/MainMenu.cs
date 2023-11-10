using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        // Getting the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reloading the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }
}
