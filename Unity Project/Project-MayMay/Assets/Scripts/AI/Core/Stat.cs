using System.Collections.Generic;
using UnityEngine;

public abstract class Stat : ScriptableObject {

<<<<<<< HEAD:Unity Project/Project-MayMay/Assets/Scripts/AI/Core/Stat.cs
    [HideInInspector]
    public Character ai;
=======
    protected Jai ai;
    public T AI<T>() where T : Jai
    {
        return ai as T;
    }
>>>>>>> 242e4cf73c44ac2fad1f9f47262ffc06f6ff1182:Unity Project/Project-MayMay/Assets/Sourcefiles/_Scripts/AI/Core/Stat.cs

    public bool saveChangesInPlayMode;
    //stats can be very generic, so that's why it isn't just an int or a float
    public abstract int GetValue();
    public abstract void SetValue(int val);
    public abstract void AddValue(int val);

    public abstract float TimeLeftUntilEmpty();
    public List<RootAction> boosters = new List<RootAction>();

    public virtual void Init(Jai ai)
    {
<<<<<<< HEAD:Unity Project/Project-MayMay/Assets/Scripts/AI/Core/Stat.cs
        this.ai = ai as Character;
=======
        this.ai = ai;
>>>>>>> 242e4cf73c44ac2fad1f9f47262ffc06f6ff1182:Unity Project/Project-MayMay/Assets/Sourcefiles/_Scripts/AI/Core/Stat.cs
    }

    protected int Uninportant
    {
        get
        {
            return 100;
        }
    }
}