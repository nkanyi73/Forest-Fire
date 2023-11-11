using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterGun : MonoBehaviour
{
    //public Transform nozzle;
    public InputActionReference shootAction;
    public ParticleSystem waterJet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shootAction.action.IsPressed())
        {
            //Debug.Log("Button Press Detected");
            StartShooting();
        }
        else
        {
            StopShooting();

        }

    }
    private void StartShooting()
    {
        waterJet.Play();

    }

    private void StopShooting() 
    { 
        if (waterJet.isPlaying == true)
        {
            waterJet.Stop();
        }
    }
}
