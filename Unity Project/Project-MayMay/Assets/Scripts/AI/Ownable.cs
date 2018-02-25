using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ownable : MonoBehaviour {

    [HideInInspector]
    public Character owner;

    public bool HasOwner
    {
        get
        {
            return owner != null;
        }
    }
}
