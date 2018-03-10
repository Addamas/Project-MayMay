using Jext;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HouseSafety : NormalBasicAction
{
    [SerializeField]
    private string houseName;

    protected House GetHouse()
    {
        return ai.GetHouse(houseName);
    }

    protected List<Door> GetDoors()
    {
        return GetHouse().doors;
    }

    protected List<Door> GetDoors(bool getOpen)
    {
        List<Door> ret = GetDoors().ConvertListToNew();
        ret.RemoveAll(x => x.IsOpen != getOpen);
        return ret;
    }

    protected Door GetDoor()
    {
        return GetDoors().SortByClosest(ai.Pos).First();
    }

    protected Door GetDoor(bool getOpen)
    {
        return GetDoors(getOpen).SortByClosest(ai.Pos).First();
    }

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override Transform PosTrans()
    {
        return GetDoor().transform;
    }

    protected override bool ExecutableCheck()
    {
        return GetHouse().doors.Count > 0;
    }
}
