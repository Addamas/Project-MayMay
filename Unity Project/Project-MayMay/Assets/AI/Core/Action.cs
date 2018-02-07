using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public bool interruptable, saveChangesInPlayMode;
    public abstract List<Jai.Requirement> GetRequirements();

    protected Jai ai;

    public virtual void Init(Jai ai)
    {
        this.ai = ai;
    }

    public abstract bool IsInRange();
    public abstract Vector3 Pos();
    public abstract void Execute();
    public abstract void Cancel();
    public virtual void Complete()
    {
        ai.curAction = null;
        ai.NewEvent();
    }

    public abstract float GetEstimatedTimeRequired();
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