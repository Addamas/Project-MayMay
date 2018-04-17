using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Water : Consumable, IComparable<Water> {

    public int CompareTo(Water other)
    {
        return other.value - value;
    }
}
