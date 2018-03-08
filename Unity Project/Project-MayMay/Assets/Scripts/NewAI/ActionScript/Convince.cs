using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "Convince", menuName = "Actions/Convince/Convince", order = 1)]
public class Convince : RootActionMulFrameable
{
    [SerializeField]
    private Stat convinceToFill;

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override int GetReturnValue()
    {
        return Max;
    }

    private int val;
    public override IEnumerator LifeTime()
    {
        Memory.Other other = GetOther();
        Character otherCharacter = other.character;
        val = GetReturnValue();

        if (otherCharacter.curAction != null)
            otherCharacter.Cancel();
        System.Type type = convinceToFill.GetType();
        foreach (Stat stat in otherCharacter.stats)
            if (stat.GetType() == type)
            {
                stat.SetValue(otherCharacter.settings.critVal);
                otherCharacter.NewEvent();
                break;
            }

        yield return null;
        Complete();
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

        for (int i = others.Count - 1; i > 0; i--)
        {
            character = others[i].character;
            action = character.curAction;
            if (action == null)
                continue;
            character.stats.Sort();
            if(!AvailableCheck(others[i]) || !action.breakable || 
                character.stats.First().GetValue() < character.settings.critVal)
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