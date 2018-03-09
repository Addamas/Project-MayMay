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

    protected override IEnumerator WhileLinked(Memory.Other other)
    {
        Social.Conversation conversation = other.conversations.Count > 0 ? 
            other.conversations.RandomItem() : Social.genericConversations.RandomItem();
        Social otherSocial = other.character.GetStat<Social>();
        Social turnTaker;

        for (int i = 0; i < conversation.parts.Count; i++)
        {
            if (other.character.curAction.GetType() != ActionType)
            {
                Complete();
                yield break;
            }
            if ((other.character.curAction as PassiveAction).leader != ai)
            {
                Complete();
                yield break;
            }

            turnTaker = i.IsEven() ? Social : otherSocial;
            yield return turnTaker.ai.StartCoroutine(turnTaker.Speak(conversation.parts[i]));
        }

        Complete();
    }
}