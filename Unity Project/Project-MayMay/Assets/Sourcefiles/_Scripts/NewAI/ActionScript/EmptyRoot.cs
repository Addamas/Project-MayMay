using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EmptyRoot", menuName = "Actions/EmptyRoot", order = 1)]
public class EmptyRoot : RootAction
{
    [SerializeField]
    private Link link;

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>() {link };
    }

    public override int GetReturnValue()
    {
        return Max;
    }

    public override Transform PosTrans()
    {
        throw new NotImplementedException();
    }

    protected override bool ExecutableCheck()
    {
        return true;
    }

    public override bool InRange()
    {
        return true;
    }
}
