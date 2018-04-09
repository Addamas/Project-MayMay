﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SearchStat", menuName = "Stats/SearchStat", order = 1)]
public class SearchStat : Stat
{
    [NonSerialized]
    public Character target;
    [NonSerialized]
    public Area targetPredictedArea;

    public delegate int ValueFunc();
    public ValueFunc valueFunc;

    public void SetTarget(Character target, Area area, ValueFunc func)
    {
        this.target = target;
        targetPredictedArea = area;
        valueFunc = func;
    }

    public void SetTarget(Character target, ValueFunc func)
    {
        this.target = target;
        valueFunc = func;
    }

    public override void Init(GHOPE ai)
    {
        base.Init(ai);
    }

    public bool Searching
    {
        get
        {
            return target != null;
        }
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
