﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NormalStat", menuName = "Stats/Generic/NormalStat", order = 1)]
public class NormalStat : Stat
{
    [SerializeField]
    private bool randomizable;

    public override void Init(GHOPE ai)
    {
        base.Init(ai);
        if(randomizable && ai.randomizeStats)
            SetValue(GameManager.random.Next((ai as Character).settings.critVal, Max));
    }

    public override void SetValue(int value)
    {
        Value = value;
        base.SetValue(value);
    }
    public override void AddValue(int value)
    {
        Value += value;
    }

    [SerializeField]
    private int value;
    protected int Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = Mathf.Clamp(value, Min, Max);
        }
    }

    public override int GetValue()
    {
        return value;
    }
}
