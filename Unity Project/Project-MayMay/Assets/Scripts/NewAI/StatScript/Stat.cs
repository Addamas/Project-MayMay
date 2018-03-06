using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Stat : Extension, IComparable<Stat> {

    public abstract int GetValue();
    public abstract void SetValue(int value);
    public abstract void AddValue(int value);

    public List<RootAction> rootActions = new List<RootAction>();

    public int CompareTo(Stat other)
    {
        return GetValue() - other.GetValue();
    }
}