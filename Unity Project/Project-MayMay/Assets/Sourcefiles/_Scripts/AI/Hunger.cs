using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hunger", menuName = "Stats/Hunger", order = 1)]
public class Hunger : Stat_Decreasing {

    public override void AddValue(int val)
    {
        base.AddValue(val);
        if (value > Uninportant)
            value = Uninportant;
    }
}