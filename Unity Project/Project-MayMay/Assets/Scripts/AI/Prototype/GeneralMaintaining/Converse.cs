using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Converse : SimpleRootAction
{
    [SerializeField]
    protected float duration;
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

    public override void Cancel()
    {
        social.conversationPartner = null;
        if (execute != null)
            ai.StopCoroutine(execute);
    }

    public override void Execute()
    {
        execute = ai.StartCoroutine(_Execute());
    }

    protected Coroutine execute;
    protected abstract IEnumerator _Execute();

    public override void Complete()
    {
        social.conversationPartner = null;
        base.Complete();
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
        //add for each sentence x time
        return ret;
    }
}
