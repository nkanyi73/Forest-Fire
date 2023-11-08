using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtinguishFire : MonoBehaviour
{
    public ForestFireCell forestFireCell;
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collision");
        forestFireCell = GetComponentInParent<ForestFireCell>();
        forestFireCell.cellFuel = 0;
        forestFireCell.SetBurnt();
    }

}
