using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boredom", menuName = "Stats/Boredom", order = 1)]
public class Boredom : Stat
{
    [SerializeField]
    private int treshold;

    public override void AddValue(int val)
    {
        
    }

    public override int GetValue()
    {
        return treshold;
    }

    public override void SetValue(int val)
    {
        
    }

    public override float TimeLeftUntilEmpty()
    {
        return Mathf.Infinity;
    }
}
