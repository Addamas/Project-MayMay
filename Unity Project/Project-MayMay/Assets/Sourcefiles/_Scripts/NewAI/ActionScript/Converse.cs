using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "Converse", menuName = "Actions/Convince/Converse", order = 1)]
public class Converse : LeadAction {

    protected Social Social
    {
        get
        {
            return ai.GetStat<Social>();
        }
    }

    protected virtual Social.Conversation PickConversation(Memory.Other other)
    {
        return GetConversation(other, Social.ConversationType.Normal);
    }

    protected virtual Social.Conversation GetConversation(Memory.Other other, Social.ConversationType type)
    {
        try
        {
            return other.GetConversation(type);
        }
        catch
        {
            return Social.GetConversation(type);
        }
    }

    protected override IEnumerator WhileLinked(Memory.Other other)
    {
        Social.Conversation conversation = PickConversation(other);
        Social otherSocial = other.character.GetStat<Social>();
        Social turnTaker;

        for (int i = 0; i < conversation.parts.Count; i++)
        {
            if(other.character.curAction == null)
            {
                ai.ForceNewEvent();
                yield break;
            }
            if (other.character.curAction.GetType() != ActionType)
            {
                ai.ForceNewEvent();
                yield break;
            }
            if ((other.character.curAction as PassiveAction).leader != ai)
            {
                ai.ForceNewEvent();
                yield break;
            }

            turnTaker = i.IsEven() ? Social : otherSocial;
            yield return turnTaker.ai.StartCoroutine(turnTaker.Speak(conversation.parts[i]));
        }

        otherSocial.AddValue(Max);
        WhenCompleted(other);
    }

    protected virtual void WhenCompleted(Memory.Other other)
    {
        Complete();
    }
}

public class ConverseNormal : LeadActionNormal
{
    protected Social Social
    {
        get
        {
            return ai.GetStat<Social>();
        }
    }

    protected virtual Social.Conversation PickConversation(Memory.Other other)
    {
        return GetConversation(other, Social.ConversationType.Normal);
    }

    protected virtual Social.Conversation GetConversation(Memory.Other other, Social.ConversationType type)
    {
        try
        {
            return other.GetConversation(type);
        }
        catch
        {
            return Social.GetConversation(type);
        }
    }

    protected override IEnumerator WhileLinked(Memory.Other other)
    {
        Social.Conversation conversation = PickConversation(other);
        Social otherSocial = other.character.GetStat<Social>();
        Social turnTaker;

        for (int i = 0; i < conversation.parts.Count; i++)
        {
            if (other.character.curAction == null)
            {
                ai.ForceNewEvent();
                yield break;
            }
            if (other.character.curAction.GetType() != ActionType)
            {
                ai.ForceNewEvent();
                yield break;
            }
            if ((other.character.curAction as PassiveAction).leader != ai)
            {
                ai.ForceNewEvent();
                yield break;
            }

            turnTaker = i.IsEven() ? Social : otherSocial;
            yield return turnTaker.ai.StartCoroutine(turnTaker.Speak(conversation.parts[i]));
        }

        otherSocial.AddValue(Max);
        WhenCompleted(other);
    }

    protected virtual void WhenCompleted(Memory.Other other)
    {
        Complete();
    }
}