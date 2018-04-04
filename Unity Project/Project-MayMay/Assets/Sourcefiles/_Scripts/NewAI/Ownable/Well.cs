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
}
