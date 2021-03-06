﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Drink", menuName = "Actions/Drink", order = 1)]
public class Drink : RootAction {

    [SerializeField]
    private string peepeeStatName;

    private bool HasWater
    {
        get
        {
            try
            {
                GetWater();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    private Water GetWater()
    {
        List<Water> ret = ai.GetFromInventory<Water>();
        ret.Sort();
        return ret[0];
    }

    public override List<Link> GetRemainingLinks()
    {
        List<Link> ret = new List<Link>();
        if (!HasWater)
            if (ai.GetAction<SearchItem>().CanFind(typeof(Water)))
                ret.Add(Link.HasItem);
            else
                ret.Add(Link.HasWater);

        return ret;
    }

    public override int GetReturnValue()
    {
        try
        {
            return GetWater().value;
        }
        catch
        {
            return 0;
        }
    }

    public override bool InRange()
    {
        return true;
    }

    protected override bool ExecutableCheck()
    {
        return true;
    }

    public override void Execute()
    {
        Water water = GetWater();
        water.Consume(Stat<TickStat>());
        ai.inventory.Remove(water);

        ai.GetStat(peepeeStatName).AddValue(Mathf.RoundToInt(water.peePerValuePoint * water.value));

        Complete();
    }

    public override void Cancel()
    {

    }

    public override void Prepare()
    {
        try
        {
            ai.GetAction<SearchItem>().target = typeof(Water);
        }
        catch
        {

        }
        try
        {
            ai.GetAction<GetFromStorage>().target = typeof(Water);
        }
        catch
        {

        }
        base.Prepare();
    }
}
