using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text timerText2;
    private float countdownTimer;
    public static bool isPaused;
    public GameObject timerCanvas;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the countdown timer
        countdownTimer = 100f;
        Debug.Log(countdownTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            if (!timerCanvas.gameObject.activeSelf)
            {
                Debug.Log("Activating Canvas");
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

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("GameOver");
    }

    void StartSwitchSceneRoutine()
    {
        StartCoroutine(SwitchScene());  
    }
}
