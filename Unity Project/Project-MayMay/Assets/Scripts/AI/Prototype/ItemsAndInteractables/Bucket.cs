using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : Item {

    public Consumable filler;

    public bool Filled
    {
        get
        {
            return filler != null;
        }
    }

    public Consumable GetFiller()
    {
        return filler;
    }

    public void SetFiller(Consumable filler)
    {
        this.filler = filler;
    }
}