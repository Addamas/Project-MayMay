using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bucket : Item {

    public Item item;

    public bool Filled
    {
        get
        {
            return item != null;
        }
    }

    public Item Empty()
    {
        Item ret = item;
        item = null;
        return ret;
    }
}
