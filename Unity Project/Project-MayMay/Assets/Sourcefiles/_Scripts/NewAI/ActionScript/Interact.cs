using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interact", menuName = "Actions/Interact", order = 1)]
public class Interact : NormalAction
{
    [NonSerialized]
    public Interactable target;

    public override void Execute()
    {
        Debug.Log(target);
        target.Interact(ai);
        Complete();
    }

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override List<Link> GetReturnValue()
    {
        return new List<Link>() {Link.Interacted };
    }

    protected override bool ExecutableCheck()
    {
        return true;
    }

    protected override void OnFinished()
    {
        target = null;
        base.OnFinished();
    }

    public override Transform PosTrans()
    {
        try
        {
            return target.transform;
        }
        catch
        {
            return base.PosTrans();
        }
    }
}
