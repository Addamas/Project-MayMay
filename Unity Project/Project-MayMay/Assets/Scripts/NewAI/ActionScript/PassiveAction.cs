﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveAction", menuName = "Actions/Convince/PassiveAction", order = 1)]
public class PassiveAction : RootActionMulFrameable
{
    [NonSerialized]
    public Character leader;

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override int GetReturnValue()
    {
        return Max;
    }

    public override IEnumerator LifeTime()
    {
        yield return null;

        while (leader.curAction != null)
        {
            if (leader.curAction as LeadAction == null)
                break;
            yield return null;
        }

        Complete();
    }

    public override float Dis()
    {
        try
        {
            return base.Dis();
        }
        catch
        {
            return 0;
        }
    }

    public override Transform PosTrans()
    {
        return leader.transform;
    }

    protected override bool ExecutableCheck()
    {
        return true;
    }

    public override void Cancel()
    {
        leader = null;
        stat.SetValue(Max);
        base.Cancel();
    }

    public override void Complete()
    {
        leader = null;
        base.Complete();
    }
}
