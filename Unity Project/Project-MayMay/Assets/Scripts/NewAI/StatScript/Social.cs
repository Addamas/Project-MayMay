using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Social", menuName = "Stats/Social", order = 1)]
public class Social : TickStat {

    [SerializeField]
    private int minValue;

    public override void SetValue(int value)
    {
        base.SetValue(Mathf.Clamp(value, minValue, Max));
    }
}
