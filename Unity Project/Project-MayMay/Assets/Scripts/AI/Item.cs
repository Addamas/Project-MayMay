using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Ownable {

    public Jai.Requirement associatedReward;

    public GameObject Obj
    {
        get
        {
            return transform.gameObject;
        }
    }
}
