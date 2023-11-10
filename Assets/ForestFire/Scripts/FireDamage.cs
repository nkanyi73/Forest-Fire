using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Dog")
        {
            other.gameObject.GetComponent<Dog>().StopBarking();
            other.gameObject.GetComponent<Dog>().Whine();
        }
    }
}
