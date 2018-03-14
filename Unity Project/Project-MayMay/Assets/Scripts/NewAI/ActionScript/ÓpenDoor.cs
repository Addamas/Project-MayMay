using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "OpenDoor", menuName = "Actions/OpenDoor", order = 1)]
public class ÓpenDoor : NormalAction {

    [SerializeField]
    protected string houseName;
    [SerializeField]
    protected List<int> openableDoorIDs = new List<int>();
    protected List<Door> doors = new List<Door>();

    public override void Init(GHOPE ai)
    {
        base.Init(ai);
        House house = (ai as Character).GetHouse(houseName);
        openableDoorIDs.ForEach(x => doors.Add(house.doors[x]));
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
        GetDoor();
        return true;
    }

    public override void Execute()
    {
        Door door = GetDoor();
        if (!door.IsOpen)
            door.Interact(ai);
        Complete();
    }

    private Door GetDoor()
    {
        List<Door> doors = this.doors.ConvertListToNew();
        doors.RemoveAll(x => x.IsOpen);
        return doors.SortByClosest(ai.Pos).First();
    }

    public override List<Link> GetReturnValue()
    {
        return new List<Link>() { Link.OpenedShop };
    }
}
