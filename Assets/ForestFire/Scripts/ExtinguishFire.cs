using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishFire : MonoBehaviour
{
    public ForestFireCell forestFireCell;
    int particleCounter;

    private void OnParticleCollision(GameObject other)
    {
       
        if (particleCounter < 10)
        {
            particleCounter++;
            //Debug.Log(particleCounter.ToString());
        }
        else
        {
            forestFireCell = GetComponentInParent<ForestFireCell>();
            Debug.Log(forestFireCell.cellState);
            forestFireCell.cellFuel = 0;
            forestFireCell.SetBurnt();
            particleCounter = 0;
        }
        
    }

}
