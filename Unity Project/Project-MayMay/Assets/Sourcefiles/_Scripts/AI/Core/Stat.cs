using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : ScriptableObject {

    protected Jai ai;
    public T AI<T>() where T : Jai
    {
        return ai as T;
    }

    public bool saveChangesInPlayMode;
    //stats can be very generic, so that's why it isn't just an int or a float
    public abstract int GetValue();
    public abstract void SetValue(int val);
    public abstract void AddValue(int val);

    public abstract float TimeLeftUntilEmpty();
    public List<RootAction> boosters = new List<RootAction>();

    public virtual void Init(Jai ai)
    {
        this.ai = ai;
    }

    protected int Uninportant
    {
        get
        {
            return 100;
        }
    }
}