using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishFire : MonoBehaviour
{
    public ForestFireCell forestFireCell; // a reference to the forest fire cell that contains different states
    int particleCounter; // a counter to keep track of the number of particles that have collided with the fire object

    private void OnParticleCollision(GameObject other)
    {
       
        if (particleCounter < 10)
        {
            // increment until the value of particle counter is not < 10
            particleCounter++;
            //Debug.Log(particleCounter.ToString());
        }
        else
        {
            //get the parent comopnent from the prefab
            forestFireCell = GetComponentInParent<ForestFireCell>();

            //Debug.Log(forestFireCell.cellState);

            //set fuel to 0 to prevent relighting and reset particleCounter to 0
            forestFireCell.cellFuel = 0;
            forestFireCell.SetBurnt();
            particleCounter = 0;
        }
        
    }

}
