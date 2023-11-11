using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FireDamage : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Dog")
        {
            other.gameObject.GetComponent<Dog>().StopBarking();
            other.gameObject.GetComponent<Dog>().Whine();
            CallCoroutine();
        }
    }

    IEnumerator SwitchScene()
    {
        
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameOver");

    }

    void CallCoroutine()
    {
        StartCoroutine(SwitchScene());
    }
}
