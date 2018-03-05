using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
=======
using Jext;
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d

public abstract class Converse : SimpleRootAction
{
    [SerializeField]
<<<<<<< HEAD
    protected float duration;
    protected Social.Other conversationPartner;
    protected Social Social
    {
        get
        {
            return STAT<Social>();
        }
    }

    protected override void ExecutableCheck()
    {
        Social.GetSocialPartner();
        base.ExecutableCheck();
=======
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
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d
    }

    public override void Cancel()
    {
<<<<<<< HEAD
        if (execute != null)
            ai.StopCoroutine(execute);
=======
        social.conversationPartner = null;
        if (execute != null)
            ai.StopCoroutine(execute);
        if (conversing != null)
            ai.StopCoroutine(conversing);
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d
    }

    public override void Execute()
    {
<<<<<<< HEAD
        if(execute != null)
            execute = ai.StartCoroutine(_Execute());
=======
        social.SetConversationPartner();
        execute = ai.StartCoroutine(_Execute());
        conversing = ai.StartCoroutine(Conversing());
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d
    }

    protected Coroutine execute;
    protected abstract IEnumerator _Execute();

    public override void Complete()
    {
<<<<<<< HEAD
        base.Complete();
        conversationPartner = null;
    }

    public override int GetReturnValue()
    {
        return GetValue();
=======
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
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d
    }

    public override Vector3 Pos()
    {
<<<<<<< HEAD
        return Social.GetSocialPartner().character.transform.position;
=======
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
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d
    }

    protected int GetValue()
    {
<<<<<<< HEAD
        if (conversationPartner == null)
            try
            {
                return Social.GetSocialPartner().affinity;
            }
            catch
            {
                return 0;
            }
        return conversationPartner.affinity;
=======
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
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d
    }
}
