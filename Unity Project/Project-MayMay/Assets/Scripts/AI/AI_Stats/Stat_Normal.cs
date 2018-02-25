using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat_Normal : Stat {

    [SerializeField]
    protected int value;

    public override void AddValue(int val)
    {
        value += val;
    }

    public override int GetValue()
    {
        return value;
    }

    public override void SetValue(int val)
    {
        value = val;
    }

    public override float TimeLeftUntilEmpty()
    {
        return Mathf.Infinity;
    }
}

public interface ITickable
{
    IEnumerator Tick();
}
