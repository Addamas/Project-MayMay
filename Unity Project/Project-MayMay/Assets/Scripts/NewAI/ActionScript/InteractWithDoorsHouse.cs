using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OpenDoors", menuName = "Actions/OpenDoors", order = 1)]
public class InteractWithDoorsHouse : HouseSafity
{
    [SerializeField]
    private bool openDoors;

    public override void Execute()
    {
        GetDoor().Interact(ai);
    }

    public override Transform PosTrans()
    {
        return GetDoor(!openDoors).transform;
    }

    protected override bool ExecutableCheck()
    {
        GetDoor(!openDoors);
        return true;
    }
}
