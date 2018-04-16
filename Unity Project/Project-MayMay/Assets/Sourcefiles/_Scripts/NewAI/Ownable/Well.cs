using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : Interactable {

    public Water water;

    public bool DriedUp
    {
        get
        {
            return water == null;
        }
    }

    public override void Interact(Character character)
    {
        Water spawnedWater = Instantiate(water, transform.position, Quaternion.identity);
        water.gameObject.SetActive(false);
        character.GetEmptyBucket().item = spawnedWater;
    }
}
