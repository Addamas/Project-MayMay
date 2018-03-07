using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : Extension
{
    #region Core Functions
    public abstract void Execute();
    public abstract void Cancel();
    public virtual void Complete()
    {
        Debug.Log(name);
        ai.Complete();
    }
    #endregion

    #region Main Check
    public enum Link {HasFood, a, b, c };
    public abstract List<Link> GetRemainingLinks();
    #endregion

    #region Small Checks
    public virtual bool IsExecuting()
    {
        return false;
    }
    public bool special;
    #endregion

    #region Executable Check
    public virtual bool IsExecutable()
    {
        try
        {
            return ExecutableCheck();
        }
        catch
        {
            return false;
        }
    }
    protected abstract bool ExecutableCheck();
    #endregion

    #region Distance
    public virtual bool InRange()
    {
        return Dis() < ai.settings.interactDistance;
    }

    public virtual float Dis()
    {
        return Vector3.Distance(ai.transform.position, Pos());
    }

    public virtual Vector3 Pos()
    {
        return PosTrans().position;
    }

    public abstract Transform PosTrans();
    #endregion

    #region Time
    public virtual float GetEstimatedTimeRequired()
    {
        if(!InRange())
            return Dis() / ai.movement.agent.speed;
        return 0;
    }
    #endregion
}

public abstract class RootAction : Action
{
    [NonSerialized]
    public Stat stat;
    public T Stat<T>() where T : Stat
    {
        return stat as T;
    }

    public virtual void Init(GHOPE ai, Stat stat)
    {
        this.stat = stat;
        base.Init(ai);
    }

    public override void Execute()
    {
        valueWhenStarted = GetReturnValue();
    }

    protected int valueWhenStarted;

    public override void Complete()
    {
        stat.AddValue(valueWhenStarted);
        base.Complete();
    }

    public abstract int GetReturnValue();
}

public abstract class NormalAction : Action
{
    public abstract List<Link> GetReturnValue();
    public abstract bool Linkable(Link link);
}

public abstract class RootActionMulFrameable : RootAction, IMultipleFramable
{
    protected Coroutine lifeTime;
    private bool executing;

    public override bool IsExecuting()
    {
        return executing;
    }

    public override void Execute()
    {
        executing = true;
        lifeTime = ai.StartCoroutine(LifeTime());
        base.Execute();
    }

    public override void Cancel()
    {
        executing = false;
        ai.StopCoroutine(lifeTime);
    }

    public override void Complete()
    {
        executing = false;
        base.Complete();
    }

    public abstract IEnumerator LifeTime();
}

public abstract class NormalActionMulFrameable : NormalAction, IMultipleFramable
{
    protected Coroutine lifeTime;

    public override void Execute()
    {
        lifeTime = ai.StartCoroutine(LifeTime());
    }

    public override void Cancel()
    {
        ai.StopCoroutine(lifeTime);
    }

    public abstract IEnumerator LifeTime();
}

public interface IMultipleFramable
{
    IEnumerator LifeTime();
}

public interface IDuration
{
    float Duration
    {
        get;
    }
}
