using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestAssistant", menuName = "Stats/QuestAssistant", order = 1)]
public class QuestAssistant : Stat {

    public string leaderName, questName;
    private Quest leaderQuest;
    public Quest LeaderQuest
    {
        get
        {
            if (leaderQuest == null)
                leaderQuest = GameManager.GetCharacter(leaderName).GetStat(questName) as Quest;
            return leaderQuest;
        }
    }

    public override void AddValue(int value)
    {
        
    }

    public override int GetValue()
    {
        return LeaderQuest.GetValue();
    }
}
