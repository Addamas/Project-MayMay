using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "CloseDoor", menuName = "Actions/CloseDoor", order = 1)]
public class CloseDoor : RootAction {

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override int GetReturnValue()
    {
        return Max;
    }

    public override Transform PosTrans()
    {
        return GetDoor().transform;
    }

    protected override bool ExecutableCheck()
    {
        GetDoor();
        return true;
    }

    public override void Execute()
    {
        Door door = GetDoor();
        if(door.IsOpen)
            door.Interact(ai);
        try
        {
            Stat<HouseManaging>().unlockedDoors.Remove(door);
        }
        catch
        {

        }
        Complete();
    }

    private Door GetDoor()
    {
        return Stat<HouseManaging>().unlockedDoors.SortByClosest(ai.Pos).First();
    }
}
