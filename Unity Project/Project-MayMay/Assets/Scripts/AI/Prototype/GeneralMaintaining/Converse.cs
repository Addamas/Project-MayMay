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

    public override void Cancel()
    {
        ai.StopCoroutine(execute);
    }

    public override void Execute()
    {
        execute = ai.StartCoroutine(_Execute());
    }

    private Coroutine execute;
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
            return Social.GetSocialPartner().affinity;
        return conversationPartner.affinity;
    }
}
