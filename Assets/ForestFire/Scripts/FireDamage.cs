using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireDamage : MonoBehaviour
{
    // method that checks if the source of collision is the Dog
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Dog")
        {
            // if source of collision is dog, stop barking and start whining and display the game over menu
            other.gameObject.GetComponent<Dog>().StopBarking();
            other.gameObject.GetComponent<Dog>().Whine();
            CallCoroutine();
        }
    }

    // coroutine to delay the loading of the game over menu
    IEnumerator SwitchScene()
    {
        
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameOver");

    }

    // method that calls the coroutine
    void CallCoroutine()
    {
        StartCoroutine(SwitchScene());
    }
}
