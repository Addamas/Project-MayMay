﻿using System.Collections;
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
        SetShortcuts();

        #region Initialize Actions and Stats
        stats.ForEach(x => x.Init(this));
        foreach (Stat stat in stats)
            stat.rootActions.ForEach(x => x.Init(this, stat));
        actions.ForEach(x => x.Init(this));
        #endregion

        movement.Init();
        memory.Init();
        senses.Init();
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

    public virtual void ForceNewEvent()
    {
        Cancel();
        NewEvent();
    }

    private bool changed;
    public virtual void NewEvent()
    {
        if (pathfinding)
            return;
        if (curAction as RootAction != null)
            if ((curAction as RootAction).GetReturnValue() <= settings.critVal)
                return;

        stats.Sort();
        if (curAction != null && stats.First().GetValue() > settings.critVal)
            return;

        pathfinding = true;
        changed = false;

        GameManager.instance.EnqueuePathfinding(this);
    }

    [NonSerialized]
    public bool pathfinding;
    public IEnumerator Pathfinding()
    {
        foreach (Stat stat in stats)
        {
            yield return StartCoroutine((PathPossible(stat)));
            if (changed)
                break;
        }

        pathfinding = false;

        if (changed)
            Execute();
    }

    public Stat FirstStat()
    {
        stats.Sort();
        return stats.First();
    }

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

    public IEnumerator PathPossible(Stat stat)
    {
        List<Path> tryable = new List<Path>(),
            pathable = new List<Path>();
        foreach (RootAction action in stat.rootActions)
            if (action.IsExecutable())
                tryable.Add(new Path(action));

        if (tryable.Count == 0)
            yield break;

        Path tryAction;
        List<Action.Link> links;

        while (tryable.Count > 0)
        {
            //remove and set ref
            tryAction = tryable.First();
            tryable.Remove(tryAction);

            //check if executable
            if (tryAction.action.GetRemainingLinks().Count == 0)
            {
                pathable.Add(tryAction);
                continue;
            }

            //get deeper path
            foreach (NormalAction action in actions)
            {
                if (!action.IsExecutable())
                    continue;

                links = tryAction.action.GetRemainingLinks();

                foreach (Action.Link link in links)
                    if (action.Linkable(link))
                    {
                        tryable.Add(new Path(action, tryAction));
                        break;
                    }
            }
        }

        if (pathable.Count == 0)
            yield break;

        pathable.Sort();

        if (curAction != null)
            if (curAction == pathable.First().action)
                yield break;

        Cancel();
        
        changed = true;
        curAction = pathable.First().action;
    }

    public virtual void Cancel()
    {
        if(curAction != null)
            curAction.Cancel();
    }
}

public class Extension : ScriptableObject
{
    public static int Min
    {
        get
        {
            return 0;
        }
    }

    public static int Max
    {
        get
        {
            return 100;
        }
    }

    public static int TooLong
    {
        get
        {
            return 10000;
        }
    }

    [NonSerialized]
    public Character ai;

    public virtual void Init(GHOPE ai)
    {
        this.ai = ai as Character;
    }
}
