using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Quest", menuName = "Stats/Quest/Quest", order = 1)]
public class Quest : Stat {

    [SerializeField]
    public List<QuestTime> times = new List<QuestTime>();

    [Serializable]
    public class QuestTime
    {
        public int start, end;
    }

    public override void AddValue(int value)
    {
        
    }

    public override int GetValue()
    {
        return IsTime() ? Min : Max;
    }

    public override void SetValue(int value)
    {
        
    }

    protected bool IsTime()
    {
        int time = TimeManager.time;
        foreach (QuestTime questTime in times)
            if (questTime.start < time && questTime.end > time)
                return true;
        return false;
    }
}
