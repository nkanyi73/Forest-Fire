using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + "Collide");
        if (other.gameObject.tag == "Dog")
        {
            Debug.Log("Damaged");
            other.gameObject.GetComponent<Dog>().Bark();
        }
    }
}
