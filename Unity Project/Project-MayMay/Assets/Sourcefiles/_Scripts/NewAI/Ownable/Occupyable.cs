using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Occupyable : Interactable {

    public bool Occupied
    {
        get
        {
            return occupant != null;
        }
    }
    [NonSerialized]
    public Character occupant;

    public void Occupy(Character character)
    {
        occupant = character;
    }

    public void UnOccupy()
    {
        occupant = null;
    }
}
