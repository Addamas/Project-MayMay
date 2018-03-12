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
            return true;
        }
    }

    protected bool IsInHouse(Character character)
    {
        return Vector3.Distance(character.Pos, transform.position) < 5; //tijdelijk obviously
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
