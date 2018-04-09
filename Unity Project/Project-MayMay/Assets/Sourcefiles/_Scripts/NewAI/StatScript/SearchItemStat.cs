using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchItemStat : Stat {

    [NonSerialized]
    public Type target;

    public delegate int ValueFunc();
    public ValueFunc valueFunc;

    public void SetTarget(Item target, ValueFunc func)
    {
        this.target = target.GetType();
        valueFunc = func;
    }

    public bool Searching
    {
        get
        {
            return target != null;
        }
    }

    public void FoundTarget()
    {
        target = null;
        valueFunc = null;
    }

    public override void AddValue(int value)
    {

    }

    public override int GetValue()
    {
        if (target == null)
            return Max;
        return valueFunc();
    }
}
