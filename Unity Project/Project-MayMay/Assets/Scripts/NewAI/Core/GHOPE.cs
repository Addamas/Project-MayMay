using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using Jext;

[RequireComponent(typeof(Memory), typeof(Movement), typeof(Senses))]
public abstract class GHOPE : MonoBehaviour {

    [NonSerialized]
    public Movement movement;
    [NonSerialized]
    public Memory memory;
    [NonSerialized]
    public Senses senses;

    public List<NormalAction> actions = new List<NormalAction>();
    public List<Stat> stats = new List<Stat>();

    public CharacterObject settings;

    [NonSerialized]
    public Action curAction;

    #region Shortcuts
    public Vector3 Pos
    {
        get
        {
            return transform.position;
        }
    }
    #endregion

    public virtual void Init()
    {
        movement.Init();
        memory.Init();
        senses.Init();
    }

    protected virtual void Awake()
    {
        SetShortcuts();

        #region Initialize Actions and Stats
        stats.ForEach(x => x.Init(this));
        foreach (Stat stat in stats)
            stat.rootActions.ForEach(x => x.Init(this, stat));
        #endregion
    }

    private void SetShortcuts()
    {
        movement = GetComponent<Movement>();
        memory = GetComponent<Memory>();
        senses = GetComponent<Senses>();

        #region Clone Scriptable Objects
        Methods.MakeCloneSOList(ref actions);
        Methods.MakeCloneSOList(ref stats);
        foreach (Stat stat in stats)
            Methods.MakeCloneSOList(ref stat.rootActions);
        #endregion
    }

    public void Complete()
    {
        memory.AddMemory(curAction);
        curAction = null;
        NewEvent();
    }

    public virtual void NewEvent()
    {
        stats.Sort();

        if (curAction != null && stats.First().GetValue() > settings.critVal)
            return;

        StopMovement();
            
        foreach (Stat stat in stats)
            if (PathPossible(stat))
                break;

        if(curAction != null)
            Execute();
    }

    public Stat FirstStat()
    {
        stats.Sort();
        return stats.First();
    }

    protected abstract void StopMovement();

    protected virtual void Execute()
    {
        curAction.Execute();
    }

    private class Path : IComparable<Path>
    {
        public Action action;
        public float duration;

        public Path(Action action)
        {
            this.action = action;
            duration = action.GetEstimatedTimeRequired();
        }

        public Path(Action action, Path path)
        {
            this.action = action;
            duration = path.duration + action.GetEstimatedTimeRequired();
        }

        public int CompareTo(Path other)
        {
            return Mathf.RoundToInt(duration - other.duration);
        }
    }

    public bool PathPossible(Stat stat)
    {
        List<Path> tryable = new List<Path>(), 
            pathable = new List<Path>();
        foreach (RootAction action in stat.rootActions)
            if (action.IsExecutable())
                tryable.Add(new Path(action));

        if (tryable.Count == 0)
            return false;

        Path tryAction;
        List<Action.Link> links;

        while(tryable.Count > 0)
        {
            //remove and set ref
            tryAction = tryable.First();
            tryable.Remove(tryAction);

            if (!tryAction.action.IsExecutable())
                continue;

            //check if executable
            if(tryAction.action.GetRemainingLinks().Count == 0)
            {
                pathable.Add(tryAction);
                continue;
            }

            //get deeper path
            foreach(NormalAction action in actions)
            {
                links = tryAction.action.GetRemainingLinks();
                foreach(Action.Link link in links)
                    if(action.Linkable(link))
                    {
                        tryable.Add(new Path(action, tryAction));
                        break;
                    }
            }
        }

        if (pathable.Count == 0)
            return false;
        pathable.Sort();

        curAction = pathable.First().action;
        return true;
    }

    public virtual void Cancel()
    {
        if(curAction != null)
            curAction.Cancel();
    }
}

public class Extension : ScriptableObject
{
    protected int Min
    {
        get
        {
            return 0;
        }
    }

    protected int Max
    {
        get
        {
            return 100;
        }
    }

    [NonSerialized]
    public Character ai;

    public virtual void Init(GHOPE ai)
    {
        this.ai = ai as Character;
    }
}
