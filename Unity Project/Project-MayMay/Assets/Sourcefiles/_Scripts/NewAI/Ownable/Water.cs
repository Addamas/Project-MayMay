using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Item {

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
