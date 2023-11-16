using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timerText; // reference to the timer's text
    public TMP_Text timerText2; // reference to the message that displays on winning
    private float countdownTimer; // timer
    public static bool isPaused; // variable that holds the game state
    public GameObject timerCanvas; // reference to the timer canvas



    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (!timerCanvas.gameObject.activeSelf)
            {
                timerCanvas.SetActive(true);
                timerText2.enabled = false;
            }
            Debug.Log(countdownTimer);
            // Update the countdown timer
            countdownTimer -= Time.deltaTime;

            // Update the displayed time
            UpdateCountdownDisplay();

            // Check if the countdown has reached zero
            if (countdownTimer <= 0f)
            {
                timerText2.enabled = true;
                StartSwitchSceneRoutine();
            }
        }
        // hide timer canvas only when it is active and the game is not paused
        if (timerCanvas.gameObject.activeSelf && isPaused)
        {
            timerCanvas.SetActive(false);
        }

    }
    void UpdateCountdownDisplay()
    {
        // Ensure the timer doesn't go below zero
        countdownTimer = Mathf.Max(0f, countdownTimer);

        int minutes = Mathf.FloorToInt(countdownTimer / 60);
        int seconds = Mathf.FloorToInt(countdownTimer % 60);

        // Update the UI Text element
        timerText.text = string.Format("Help arriving in: {0:00}:{1:00}", minutes, seconds);
    }

    // coroutine to delay redirect to game over menu
    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("GameOver");
    }

    // method that starts the coroutine
    void StartSwitchSceneRoutine()
    {
        StartCoroutine(SwitchScene());  
    }
}
