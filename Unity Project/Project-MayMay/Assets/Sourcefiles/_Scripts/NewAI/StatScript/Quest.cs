using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Quest", menuName = "Stats/Quest/Quest", order = 1)]
public class Quest : Stat {

    [SerializeField]
    public List<QuestTime> times = new List<QuestTime>();

    [SerializeField]
    private int questImportance = 10;

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
        return IsTime() ? questImportance : Max;
    }

    public override void SetValue(int value)
    {
        
    }

    public bool IsTime()
    {
        int time = TimeManager.time;
        foreach (QuestTime questTime in times)
            if (questTime.start < time && questTime.end > time)
                return true;
        return false;
    }
}
