using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HouseManagement", menuName = "Stats/HouseManagement", order = 1)]
public class HouseManagement : Stat
{
    public override void AddValue(int value)
    {
        
    }

    public override int GetValue()
    {
        List<House> houses = ai.GetFromInteractables<House>();
        foreach (House house in houses)
            if (!house.Locked)
                return ai.settings.critVal;
        return Max;
    }

    public override void SetValue(int value)
    {
        
    }
}
