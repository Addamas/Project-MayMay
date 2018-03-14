using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Consumable : Item, IComparable<Consumable>
{
    public int value;
    public float peePerValuePoint;

    public virtual void Consume(TickStat stat)
    {
        stat.AddValue(value);
    }

    public int CompareTo(Consumable other)
    {
        return other.value - value;
    }
}