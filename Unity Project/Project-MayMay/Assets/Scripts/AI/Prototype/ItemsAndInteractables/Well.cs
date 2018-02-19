using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : GrantConsumables {

    [SerializeField]
    private Water water;

    public override Consumable GetConsumable()
    {
        return water;
    }
}
