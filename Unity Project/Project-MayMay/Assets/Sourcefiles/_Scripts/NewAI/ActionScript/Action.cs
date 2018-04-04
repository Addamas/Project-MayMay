using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : Extension
{
    #region Core Functions
    public abstract void Execute();

    public virtual void Cancel()
    {
        if(ai.debug)
            Debug.Log("CANCELLED: " + name + " " + ai + " " + TimeManager.time);
        OnFinished();
        ai.curAction = null;
    }
    public virtual void Complete()
    {
        if(ai.debug)
            Debug.Log("COMPLETED: " + name + " " + ai + " " + TimeManager.time);
        OnFinished();
        ai.Complete();
    }

    protected virtual void OnFinished() { }
    #endregion

    #region Main Check
    public enum Link {HasFood, hasWater, OpenedShop, hasBucket, hasFilledBucket };
    public abstract List<Link> GetRemainingLinks();
    #endregion

    #region Small Checks
    public bool special, breakable, autoMovement;

    public virtual bool IsExecuting()
    {
        return false;
    }

    protected List<Memory.Other> Spotting()
    {
        List<Memory.Other> surrounding = ai.senses.GetSurrounding();
        surrounding.RemoveAll(x => !x.character.senses.TrySpot(ai));
        return surrounding;
    }
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

    public bool InRange(Transform other)
    {
        return Vector3.Distance(ai.transform.position, other.position) < StoppingDistance();
    }

    public virtual float Dis()
    {
        return Vector3.Distance(ai.transform.position, Pos());
    }

    public virtual Vector3 Pos()
    {
        return PosTrans().position;
    }

    public virtual Transform PosTrans()
    {
        return ai.transform;
    }

    #endregion

    #region Time
    public virtual float GetEstimatedTimeRequired()
    {
        if(!InRange())
            return Dis() / ai.movement.agent.speed;
        return 0;
    }
    #endregion

    public virtual void WhileMoving()
    {

    }

    public virtual float StoppingDistance()
    {
        return ai.settings.interactDistance;
    }
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
    public virtual bool Linkable(Link link)
    {
        if (GetReturnValue().Contains(link))
            return true;
        return false;
    }
}

public abstract class NormalBasicAction : NormalAction
{
    [SerializeField]
    protected List<Link> returnValue = new List<Link>();

    public override List<Link> GetReturnValue()
    {
        return returnValue;
    }
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
        if(lifeTime != null)
            ai.StopCoroutine(lifeTime);
        base.Cancel();
    }

    public override void Complete()
    {
        executing = false;
        if (lifeTime != null)
            ai.StopCoroutine(lifeTime);
        base.Complete();
    }

    public abstract IEnumerator LifeTime();
}

public abstract class NormalActionMulFrameable : NormalAction, IMultipleFramable
{
    protected Coroutine lifeTime;
    private bool executing;

    public override void Execute()
    {
        executing = true;
        lifeTime = ai.StartCoroutine(LifeTime());
    }

    public override void Cancel()
    {
        if (lifeTime != null)
            ai.StopCoroutine(lifeTime);
        executing = false;
        base.Cancel();
    }

    public override void Complete()
    {
        executing = false;
        base.Complete();
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
