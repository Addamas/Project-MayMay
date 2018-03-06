using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;
using Jext;

[RequireComponent(typeof(Movement))]
public abstract class GHOPE : MonoBehaviour {

    [NonSerialized]
    public Movement movement;

    public List<NormalAction> actions = new List<NormalAction>();
    public List<Stat> stats = new List<Stat>();

    public int critVal = 10;

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

        #region Clone Scriptable Objects
        Methods.MakeCloneSOList(ref actions);
        Methods.MakeCloneSOList(ref stats);
        foreach (Stat stat in stats)
            Methods.MakeCloneSOList(ref stat.rootActions);
        #endregion
    }

    public void Complete()
    {
        curAction = null;
        NewEvent();
    }

    protected bool Unbreakable
    {
        get
        {
            return curAction != null && stats.First().GetValue() > critVal;
        }
    }

    public virtual void NewEvent()
    {
        stats.Sort();

        if (Unbreakable)
            return;

        StopMovement();
            
        foreach (Stat stat in stats)
            if (PathPossible(stat))
                break;

        if(curAction != null)
            Execute();
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
        stat.rootActions.ForEach(x => tryable.Add(new Path(x)));

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

    protected Character ai;

    public virtual void Init(GHOPE ai)
    {
        this.ai = ai as Character;
    }
}
