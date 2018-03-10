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
}
