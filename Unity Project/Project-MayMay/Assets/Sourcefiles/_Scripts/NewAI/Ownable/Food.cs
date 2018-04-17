using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Food : Consumable, IComparable<Food>
{
    public int CompareTo(Food other)
    {
        return other.value - value;
    }
}