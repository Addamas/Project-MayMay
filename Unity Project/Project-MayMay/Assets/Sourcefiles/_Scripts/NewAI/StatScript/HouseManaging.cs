using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HouseManaging", menuName = "Stats/HouseManaging", order = 1)]
public class HouseManaging : Stat
{
    [NonSerialized]
    public List<Door> unlockedDoors = new List<Door>();

    public override void AddValue(int value)
    {
        
    }

    public override int GetValue()
    {
        return unlockedDoors.Count > 0 ? ai.settings.critVal : Max;
    }
}
