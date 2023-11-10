using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    public static int difficultyLevel;
    public GameObject mainCanvas;
    public GameObject settingsCanvas;


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

    public void BackButton()
    {
        SetDifficultyLevel(0);
        settingsCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        //Debug.Log(difficultyLevel + "Back");
    }

    public void SelectDifficultyButton()
    {
        settingsCanvas.SetActive(true);
        mainCanvas.SetActive(false);
    }
    
    void SetDifficultyLevel(int difficultyValue)
    {
        difficultyLevel = difficultyValue;
    }

    public void SetHardDifficulty()
    {
        SetDifficultyLevel(1);
        settingsCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        //Debug.Log(difficultyLevel + "Hard");
    }
    
}
