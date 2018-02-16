﻿using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    protected Character ai;

    public bool interruptable, saveChangesInPlayMode;
    public abstract List<Jai.Requirement> GetRequirements();

    public bool executableCheck;
    public bool Executable
    {
        get
        {
            if (!executableCheck)
                return true;
            try
            {
                ExecutableCheck();
                return true;
            }
            catch
            {
                Debug.Log(name + " cannot be executed");
                return false;
            }
        }
    }

    protected virtual void ExecutableCheck()
    {
        Pos();
    }

    public virtual void Init(Jai ai)
    {
        this.ai = ai as Character;
    }

    public virtual bool IsInRange()
    {
        try
        {
            return Dis() < ai.interactDistance;
        }
        catch
        {
            return false;
        }
    }
    public abstract Vector3 Pos();
    public virtual float Dis()
    {
        return Vector3.Distance(ai.transform.position, Pos());
    }
    public abstract void Execute();
    public abstract void Cancel();
    public virtual void Complete()
    {
        ai.curAction = null;
        ai.NewEvent();
    }

    public virtual float GetEstimatedTimeRequired()
    {
        try
        {
            return  Dis() / ai.agent.speed;
        }
        catch
        {
            return Mathf.Infinity;
        }
    }

    protected int Uninportant
    {
        get
        {
            return 100;
        }
    }

    protected int Important
    {
        get
        {
            return 0;
        }
    }
}

public abstract class NormalAction : Action
{
    public abstract List<Jai.Requirement> GetRewards();

    public override void Complete()
    {
        List<Jai.Requirement> ret = GetRewards();
        foreach (Jai.Requirement s in ret)
            if (!ai.filledRequirements.Contains(s))
                ai.filledRequirements.Add(s);
        base.Complete();    
    }
}

public abstract class RootAction : Action
{
    protected Stat stat;
    protected T STAT<T>() where T : Stat
    {
        return stat as T;
    }

    public abstract int GetReturnValue();

    public virtual void Init(Jai ai, Stat stat)
    {
        Init(ai);
        this.stat = stat;
    }

    public override void Complete()
    {
        stat.AddValue(GetReturnValue());
        base.Complete();
    }
}

#region Simplifiers
public abstract class SimpleNormalAction : NormalAction
{
    public List<Jai.Requirement> requirements = new List<Jai.Requirement>(),
        rewards = new List<Jai.Requirement>();

    public override List<Jai.Requirement> GetRequirements()
    {
        return requirements;
    }

    public override List<Jai.Requirement> GetRewards()
    {
        return rewards;
    }
}

public abstract class SimpleRootAction : RootAction
{
    public List<Jai.Requirement> requirements = new List<Jai.Requirement>();

    public override List<Jai.Requirement> GetRequirements()
    {
        return requirements;
    }
}
#endregion