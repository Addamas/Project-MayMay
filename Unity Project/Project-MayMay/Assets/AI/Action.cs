using System.Collections.Generic;
using UnityEngine;

public abstract class Action : ScriptableObject
{
    public bool interruptable, saveChangesInPlayMode;
    public abstract List<string> GetRequirements();

    protected Jai ai;

    public virtual void Init(Jai ai)
    {
        this.ai = ai;
    }

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
    public abstract List<string> GetRewards();

    public override void Complete()
    {
        List<string> ret = GetRewards();
        foreach (string s in ret)
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
    public List<string> requirements = new List<string>(),
        rewards = new List<string>();

    public override List<string> GetRequirements()
    {
        return requirements;
    }

    public override List<string> GetRewards()
    {
        return rewards;
    }
}

public abstract class SimpleRootAction : RootAction
{
    public List<string> requirements = new List<string>();

    public override List<string> GetRequirements()
    {
        return requirements;
    }
}
#endregion