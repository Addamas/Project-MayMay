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
        character.GetEmptyBucket().item = water;
    }
}
