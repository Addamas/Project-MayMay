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

        while (otherCharacter.pathfinding)
            yield return null;

        if (otherCharacter.curAction != null)
            otherCharacter.Cancel();

        BeforeNewEventOther(otherCharacter);
        ProcessStat(GetWantedAction(otherCharacter).stat);
        otherCharacter.NewEvent();

        while (otherCharacter.pathfinding)
            yield return null;

        lifeTime = ai.StartCoroutine(SecondLifeTime(other));
        yield break;
    }

    protected virtual void BeforeNewEventOther(Character other)
    {
        GetWantedAction(other).leader = ai;
    }

    protected virtual void ProcessStat(Stat stat)
    {
        stat.SetValue(0);
    }

    protected PassiveAction GetWantedAction(Character character)
    {
        foreach (Stat stat in character.stats)
            foreach (RootAction action in stat.rootActions)
                if (action.GetType() == ActionType)
                    return action as PassiveAction;
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

    [SerializeField]
    protected bool rangeMatters = true;
    protected virtual List<Memory.Other> GetAvailable()
    {
        List<Memory.Other> others = new List<Memory.Other>();
        if (rangeMatters)
            others = ai.senses.GetSurrounding();
        else
            others = ai.memory.relatives.ConvertListToNew();
        Action action;
        Character character;

        int count = others.Count - 1;
        for (int i = count; i > -1; i--)
        {
            character = others[i].character;
            action = character.curAction;

            if (!AvailableCheck(others[i]))
            {
                others.RemoveAt(i);
                continue;
            }

            if (action == null)
                continue;

            character.stats.Sort();
            if (!action.breakable || character.stats.First().GetValue() < character.settings.critVal)
                others.RemoveAt(i);
        }

        others.SuperSort(SortOthers);
        return others;
    }

    protected virtual Memory.Other GetOther()
    {
        return GetAvailable().First();
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

public class ConvinceNormal : NormalActionMulFrameable
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

    public override IEnumerator LifeTime()
    {
        Memory.Other other = GetOther();
        Character otherCharacter = other.character;

        while (otherCharacter.pathfinding)
            yield return null;

        if (otherCharacter.curAction != null)
            otherCharacter.Cancel();

        BeforeNewEventOther(otherCharacter);
        ProcessStat(GetWantedAction(otherCharacter).stat);
        otherCharacter.NewEvent();

        while (otherCharacter.pathfinding)
            yield return null;

        lifeTime = ai.StartCoroutine(SecondLifeTime(other));
        yield break;
    }

    protected virtual void BeforeNewEventOther(Character other)
    {
        GetWantedAction(other).leader = ai;
    }

    protected virtual void ProcessStat(Stat stat)
    {
        stat.SetValue(stat.ai.settings.critVal - 1);
    }

    protected PassiveAction GetWantedAction(Character character)
    {
        foreach (Stat stat in character.stats)
            foreach (RootAction action in stat.rootActions)
                if (action.GetType() == ActionType)
                    return action as PassiveAction;
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

    [SerializeField]
    protected bool rangeMatters = true;
    protected virtual List<Memory.Other> GetAvailable()
    {
        List<Memory.Other> others = new List<Memory.Other>();
        if (rangeMatters)
            others = ai.senses.GetSurrounding();
        else
            others = ai.memory.relatives.ConvertListToNew();
        Action action;
        Character character;

        int count = others.Count - 1;
        for (int i = count; i > -1; i--)
        {
            character = others[i].character;
            action = character.curAction;

            if (!AvailableCheck(others[i]))
            {
                others.RemoveAt(i);
                continue;
            }

            if (action == null)
                continue;

            character.stats.Sort();
            if (!action.breakable || character.stats.First().GetValue() < character.settings.critVal)
                others.RemoveAt(i);
        }

        others.SuperSort(SortOthers);
        return others;
    }

    protected virtual Memory.Other GetOther()
    {
        return GetAvailable().First();
    }

    private float SortOthers(Memory.Other other)
    {
        return Vector3.Distance(other.character.Pos, ai.Pos);
    }

    protected virtual bool AvailableCheck(Memory.Other other)
    {
        return true;
    }

    public override List<Link> GetReturnValue()
    {
        return new List<Link>();
    }
}