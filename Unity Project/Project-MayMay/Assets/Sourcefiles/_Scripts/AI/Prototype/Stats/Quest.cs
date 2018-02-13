using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Stats/Quest", order = 1)]
public class Quest : Stat
{
    [SerializeField]
    private int openTime, endTime;

    public override void AddValue(int val)
    {
        
    }

    public override int GetValue()
    {
        if (IsTime())
            return 0;
        return Uninportant;
    }

    public bool IsTime()
    {
        float curTime = Gamemanager.instance.time;
        if (curTime > openTime && curTime < endTime)
            return true;
        return false;
    }

    public override void SetValue(int val)
    {
        
    }

    public override float TimeLeftUntilEmpty()
    {
        return Mathf.Infinity;
    }
}
