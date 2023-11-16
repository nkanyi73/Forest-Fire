using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public AudioSource dogBark; // audio source representing dog's bark
    public AudioSource dogWhine; // audio source representing dog's whine

    //method that plays the dog's whine
    public void Whine()
    {
        dogWhine.Play();
    }

    //method that plays the dog's bark
    public void Bark()
    {
        dogBark.Play();
    }
    //method that stops the dog's bark
    public void StopBarking ()
    {
        dogBark.Stop();
    }
}
