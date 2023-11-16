using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterGun : MonoBehaviour
{
    public InputActionReference shootAction; // reference to the shoot control
    public ParticleSystem waterJet; // reference to the particle system that represents water particles

    // Update is called once per frame
    void Update()
    {
        // start shooting as long as shoot action is pressed
        if (shootAction.action.IsPressed())
        {
            //Debug.Log("Button Press Detected");
            StartShooting();
        }
        // else stop shooting
        else
        {
            StopShooting();

        }

    }
    private void StartShooting()
    {
        waterJet.Play();

    }

    // only stop the partcle system's playing when it has been detected that it is playing
    private void StopShooting() 
    { 
        if (waterJet.isPlaying == true)
        {
            waterJet.Stop();
        }
    }
}
