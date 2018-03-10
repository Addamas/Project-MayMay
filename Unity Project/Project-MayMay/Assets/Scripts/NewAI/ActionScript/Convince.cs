using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "Convince", menuName = "Actions/Convince/Convince", order = 1)]
public class Convince : RootActionMulFrameable
{
    [SerializeField]
    protected PassiveAction wantedAction;
    protected System.Type ActionType
    {
        get
        {
            return wantedAction.GetType();
        }
    }

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override int GetReturnValue()
    {
        return Max;
    }

    public override IEnumerator LifeTime()
    {
        Memory.Other other = GetOther();
        Character otherCharacter = other.character;

        if (otherCharacter.curAction != null)
            otherCharacter.Cancel();

        ProcessStat(GetWantedAction(otherCharacter).stat);
        otherCharacter.NewEvent();

        lifeTime = ai.StartCoroutine(SecondLifeTime(other));
        yield break;
    }

    protected virtual void ProcessStat(Stat stat)
    {
        stat.SetValue(stat.ai.settings.critVal);
    }

    protected RootAction GetWantedAction(Character character)
    {
        foreach (Stat stat in character.stats)
            foreach (RootAction action in stat.rootActions)
                if (action.GetType() == ActionType)
                    return action;
        return null;
    }

    protected virtual IEnumerator SecondLifeTime(Memory.Other other)
    {
        Complete();
        yield break;
    }

    public override Transform PosTrans()
    {
        return GetOther().character.transform;
    }

    protected override bool ExecutableCheck()
    {
        return GetAvailable().Count > 0;
    }

    protected virtual List<Memory.Other> GetAvailable()
    {
        List<Memory.Other> others = ai.senses.GetSurrounding();
        Action action;
        Character character;

        int count = others.Count - 1;
        for (int i = count; i > -1; i--)
        {
            character = others[i].character;
            action = character.curAction;
            
            if (action == null)
                continue;

            character.stats.Sort();
            if (!AvailableCheck(others[i]) || !action.breakable ||
                character.stats.First().GetValue() <= character.settings.critVal)
                others.RemoveAt(i);
        }

        others.SuperSort(SortOthers);
        return others;
    }

    protected virtual Memory.Other GetOther()
    {
        return GetAvailable()[0];
    }

    private float SortOthers(Memory.Other other)
    {
        return Vector3.Distance(other.character.Pos, ai.Pos);
    }

    protected virtual bool AvailableCheck(Memory.Other other)
    {
        return true;
    }
}