using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Ownable {

	public Item Pickup()
    {
        Destroy(transform);
        return this;
    }
}
