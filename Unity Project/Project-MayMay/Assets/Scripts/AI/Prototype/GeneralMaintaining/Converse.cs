using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Converse : SimpleRootAction
{
    [SerializeField]
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
    }

    public override void Cancel()
    {
        if (execute != null)
            ai.StopCoroutine(execute);
    }

    public override void Execute()
    {
        if(execute != null)
            execute = ai.StartCoroutine(_Execute());
    }

    protected Coroutine execute;
    protected abstract IEnumerator _Execute();

    public override void Complete()
    {
        base.Complete();
        conversationPartner = null;
    }

    public override int GetReturnValue()
    {
        return GetValue();
    }

    public override Vector3 Pos()
    {
        return Social.GetSocialPartner().character.transform.position;
    }

    protected int GetValue()
    {
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
    }
}
