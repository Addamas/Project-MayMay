using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Stat : Extension, IComparable<Stat> {

    public abstract int GetValue();
    public virtual void SetValue(int value)
    {
        if (GetValue() < ai.settings.critVal)
            ai.NewEvent();
    }
    public abstract void AddValue(int value);

    public List<RootAction> rootActions = new List<RootAction>();

    public int CompareTo(Stat other)
    {
        return GetValue() - other.GetValue();
    }

    public T GetAction<T>() where T : RootAction
    {
        foreach (RootAction action in rootActions)
            if (action.GetType() is T)
                return action as T;
        return null;
    }
}