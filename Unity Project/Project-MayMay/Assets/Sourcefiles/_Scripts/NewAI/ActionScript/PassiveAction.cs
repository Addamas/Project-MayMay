using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveAction", menuName = "Actions/Convince/PassiveAction", order = 1)]
public class PassiveAction : NormalActionMulFrameable
{
    [NonSerialized]
    public Character leader;

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override List<Link>  GetReturnValue()
    {
        return new List<Link> { Link.Passivity };
    }

    public override IEnumerator LifeTime()
    {
        while (leader.curAction != null)
        {
            if (leader.curAction as LeadAction == null && 
                leader.curAction as LeadActionNormal == null)
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
        return leader != null;
    }

    protected override void OnFinished()
    {
        leader = null;
        base.OnFinished();
    }
}
