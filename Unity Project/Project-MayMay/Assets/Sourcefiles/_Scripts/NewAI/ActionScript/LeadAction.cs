using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LeadAction", menuName = "Actions/Convince/LeadAction", order = 1)]
public class LeadAction : Convince {

    protected override IEnumerator SecondLifeTime(Memory.Other other)
    {
        Character otherCharacter = other.character;
        PassiveAction action = otherCharacter.curAction as PassiveAction;

        bool fit = true;
        if (action == null)
            fit = false;
        else if (action.GetType() != ActionType)
            fit = false;

        if (!fit)
        {
            ai.ForceNewEvent();
            yield break;
        }

        lifeTime = ai.StartCoroutine(WhileLinked(other));
    }

    protected virtual IEnumerator WhileLinked(Memory.Other other)
    {
        while (other.character.curAction.GetType() == ActionType)
            if ((other.character.curAction as PassiveAction).leader != ai)
                break;
            else
                yield return null;

        Complete();
    }
}

public class LeadActionNormal : ConvinceNormal
{
    protected override IEnumerator SecondLifeTime(Memory.Other other)
    {
        Character otherCharacter = other.character;
        PassiveAction action = otherCharacter.curAction as PassiveAction;

        bool fit = true;
        if (action == null)
            fit = false;
        else if (action.GetType() != ActionType)
            fit = false;

        if (!fit)
        {
            ai.ForceNewEvent();
            yield break;
        }

        lifeTime = ai.StartCoroutine(WhileLinked(other));
    }

    protected virtual IEnumerator WhileLinked(Memory.Other other)
    {
        while (other.character.curAction.GetType() == ActionType)
            if ((other.character.curAction as PassiveAction).leader != ai)
                break;
            else
                yield return null;

        Complete();
    }
}