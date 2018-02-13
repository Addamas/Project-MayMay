using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Quest", menuName = "Stats/Quest", order = 1)]
public class Quest : Stat
{
    [SerializeField]
    private Moment[] moments;

    [Serializable]
    public struct Moment
    {
        public int start, end;
    }

    public override void AddValue(int val)
    {
        
    }

    public override int GetValue()
    {
        if (IsTime())
            return ai.criticalLevel;
        return Uninportant;
    }

    public bool IsTime()
    {
        float curTime = Gamemanager.time;
        foreach(Moment moment in moments)
            if (curTime > moment.start && curTime < moment.end)
                return true;
        return false;
    }

    public Moment GetMoment()
    {
        float curTime = Gamemanager.time;
        foreach (Moment moment in moments)
            if (curTime > moment.start && curTime < moment.end)
                return moment;
        return moments[0];
    }

    public float GetDuration()
    {
        Moment moment = GetMoment();
        float ret = moment.end - moment.start, ret2 = moment.end - Gamemanager.time;
        return ret < ret2 ? ret : ret2;
    }

    public override void SetValue(int val)
    {
        
    }

    public override float TimeLeftUntilEmpty()
    {
        return Mathf.Infinity;
    }
}
