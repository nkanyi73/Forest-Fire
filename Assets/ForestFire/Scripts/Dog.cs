using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public AudioSource dogBark;
    /*public AudioSource dogWhine*/
    //public ForestFire3D forestFire3D;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   public void Bark()
    {
        dogBark.Play();
    }
}
