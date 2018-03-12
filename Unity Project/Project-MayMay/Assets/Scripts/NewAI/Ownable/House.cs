using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Interactable {

    public List<Door> doors = new List<Door>();

    public bool Locked
    {
        get
        {
            foreach (Door door in doors)
                if (door.IsOpen)
                    return false;
            return true;
        }
    }

    public bool UnwantedUnlock
    {
        get
        {
            foreach (Character owner in owners)
                if (IsInHouse(owner))
                    return false;
            if (UnlockedDoors.Count > 0)
                return true;
            return false;
        }
    }

    public List<Door> UnlockedDoors {
        get
        {
            List<Door> ret = new List<Door>();
            foreach (Door door in doors)
                if (door.IsOpen)
                    ret.Add(door);
            return ret;
        }
    }

    protected bool IsInHouse(Character character)
    {
        return Vector3.Distance(character.Pos, transform.position) < 1; //tijdelijk obviously
    }

    public bool Open
    {
        get
        {
            foreach (Door door in doors)
                if (!door.IsOpen)
                    return false;
            return true;
        }
    }
}
