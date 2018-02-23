using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

public abstract class Converse : SimpleRootAction
{
    [SerializeField]
    protected float timePerChar = 0.1f;
    protected Social social;

    public override void Init(Jai ai, Stat stat)
    {
        base.Init(ai, stat);
        social = STAT<Social>();
    }

    protected override bool ExecutableCheck()
    {
        Social other;
        try
        {
            other = social.GetAssociate().Social;
        }
        catch
        {
            other = social.GetRandom();
        }

        if (other.Conversing)
            return false;
        if (!base.ExecutableCheck())
            return false;

        return !social.Conversing;
    }

    protected Coroutine conversing;
    protected virtual IEnumerator Conversing()
    {
        //choose conversation
        Character.Conversation conversation;
        try
        {
            conversation = social.conversationPartner.conversations.RandomItem();
        }
        catch
        {
            conversation = ai.defaultConversation;
        }

        //execute conversation
        //temp
        bool self = true;
        foreach(Character.ConversationPart part in conversation.data)
        {
            foreach(string sentence in part.data)
            {
                Debug.Log((self ? ai.name : social.conversationPartner.character.name) + ": " + sentence);
                yield return new WaitForSeconds(sentence.Length * timePerChar);
            }
            self = !self;
        }

        Complete();
    }

    public override void Cancel()
    {
        social.conversationPartner = null;
        if (execute != null)
            ai.StopCoroutine(execute);
        if (conversing != null)
            ai.StopCoroutine(conversing);
    }

    public override void Execute()
    {
        social.SetConversationPartner();
        execute = ai.StartCoroutine(_Execute());
        conversing = ai.StartCoroutine(Conversing());
    }

    protected Coroutine execute;
    protected abstract IEnumerator _Execute();

    public override void Complete()
    {
        if (execute != null)
            ai.StopCoroutine(execute);
        RewardOther();
        social.conversationPartner.Social.conversationPartner = null;
        social.conversationPartner = null;
        base.Complete();
    }

    protected virtual void RewardOther()
    {
        social.conversationPartner.Social.AddValue(Uninportant);
    }

    public override Vector3 Pos()
    {
        if(social.conversationPartner == null)
            try
            {
                return social.GetAssociate().Pos;
            }
            catch
            {
                return social.GetRandom().Pos;
            }
        return social.conversationPartner.Pos;
    }

    protected int GetValue()
    {
        if (social.conversationPartner == null)
            try
            {
                social.GetAssociate();
            }
            catch
            {
                try
                {
                    social.GetRandom();
                }
                catch
                {
                    return 0;
                }
            }

        return Uninportant;
    }

    public override float GetEstimatedTimeRequired()
    {
        float ret = base.GetEstimatedTimeRequired();
        ret += ai.defaultConversation.Duration * timePerChar;
        return ret;
    }
}
