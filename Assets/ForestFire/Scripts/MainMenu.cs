using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static int difficultyLevel; // static variable to represent the difficulty level 1 to represent hard and 0 normal
    public GameObject mainCanvas; // canvas that represents the main menu
    public GameObject settingsCanvas; // canvas that represents the menu for difficulty selection

    // method that fires when the play button is pressed
    public void PlayGame()
    {
        SceneManager.LoadScene("ForestFire3D");
    }

    //method that fires when the quit game button is pressed
    public void QuitGame()
    {
        Application.Quit();
    }

    // method that fires when the restart game button is pressed
    public void RestartGame()
    {
        // Getting the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reloading the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }

    // method that fires when the back button is pressed
    public void BackButton()
    {
        SetDifficultyLevel(0);
        settingsCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        //Debug.Log(difficultyLevel + "Back");
    }

    // method that fires when the select difficulty button is pressed
    public void SelectDifficultyButton()
    {
        settingsCanvas.SetActive(true);
        mainCanvas.SetActive(false);
    }
    
    // methods that are used to set the difficulty
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
