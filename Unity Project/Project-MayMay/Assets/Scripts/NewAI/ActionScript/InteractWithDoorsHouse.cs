﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OpenDoors", menuName = "Actions/OpenDoors", order = 1)]
public class InteractWithDoorsHouse : HouseSafety
{
    [SerializeField]
    private bool openDoors;

    public override void Execute()
    {
        GetDoor().Interact(ai);
        Complete();
    }

    public override Transform PosTrans()
    {
        return GetDoor(!openDoors).transform;
    }

    public override bool Linkable(Link link)
    {
        return base.Linkable(link);
    }

    protected override bool ExecutableCheck()
    {
        GetDoor(!openDoors);
        return true;
    }
}
