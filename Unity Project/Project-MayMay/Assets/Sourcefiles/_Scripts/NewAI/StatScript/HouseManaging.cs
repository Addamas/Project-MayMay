using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "HouseManaging", menuName = "Stats/HouseManaging", order = 1)]
public class HouseManaging : Stat
{
    [NonSerialized]
    public List<Door> unlockedDoors = new List<Door>();
    [SerializeField]
    private float threshold = 10;

    public override void AddValue(int value)
    {
        
    }

    public override int GetValue()
    {
        unlockedDoors.SortByClosest(ai.Pos);
        foreach (Door door in unlockedDoors)
            if (Vector3.Distance(door.transform.position, ai.Pos) > threshold)
                return ai.settings.critVal;
        return Max;
    }
}
