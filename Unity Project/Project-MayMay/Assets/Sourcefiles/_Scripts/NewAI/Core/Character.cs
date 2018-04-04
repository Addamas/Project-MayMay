﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;
using System;

/*
    TODAY:
    Social
    Memory

    Quest

    actions:
    converse

    bored actions, sit, converse, play, idk
    gotobed
    report
    investigate

    stats:

    tiredness
    morale
    job - innkeeper, cook, servant, thief, police
*/

public class Character : GHOPE {

    public bool debug;

    #region Get Functions


    public T GetAction<T>() where T : Action
    {
        foreach (Action action in actions)
            if (action is T)
                return action as T;
        foreach (Stat stat in stats)
            foreach (RootAction action in stat.rootActions)
                if (action is T)
                    return action as T;
        return null;
    }

    public Action GetAction(string name)
    {
        foreach (Action action in actions)
            if (action.name == name + "(Clone)")
                return action;
        foreach (Stat stat in stats)
            foreach (RootAction action in stat.rootActions)
                if (action.name == name + "(Clone)")
                    return action;
        return null;
    }

    public T GetStat<T>() where T : Stat
    {
        foreach (Stat stat in stats)
            if (stat is T)
                return stat as T;
        return null;
    }

    public Stat GetStat(string name)
    {
        name += "(Clone)";
        foreach (Stat stat in stats)
            if (stat.name == name)
                return stat;
        return null;
    }

    public Stat GetStat(Type type)
    {
        foreach (Stat stat in stats)
            if (type == stat.GetType())
                return stat;
        return null;
    }

    public Area GetArea()
    {
        List<Area> districts = new List<Area>();
        GameManager.districts.ForEach(x => districts.Add(x));

        districts = districts.SortByClosest(Pos);
        return districts.First().GetClosestInDistrict(Pos);
    }

    #endregion

    #region Checks
    public bool Socializable
    {
        get
        {
            try
            {
                GetStat<Social>();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
    #endregion

    #region Inventory & Available Interactables
    public List<Item> inventory = new List<Item>();
    public List<Interactable> interactables = new List<Interactable>();

    public override void Init()
    {
        #region Add Owner
        inventory.ForEach(x => x.owners.Add(this));
        interactables.ForEach(x => x.owners.Add(this));
        #endregion

        base.Init();
    }

    public List<T> GetFromInventory<T>() where T : Item
    {
        return inventory.GetTypeFromListAsU<Item, T>();
    }

    public List<T> GetFromInteractables<T>() where T : Interactable
    {
        List<T> ret = interactables.GetTypeFromListAsU<Interactable, T>();
        foreach (T t in ret)
            if (ret as Occupyable != null)
                if ((ret as Occupyable).Occupied)
                    ret.Remove(t);
        ret.SortByClosest(Pos);
        return ret;
    }

    public House GetHouse(string houseName)
    {
        List<House> houses = GetFromInteractables<House>();

        foreach (House house in houses)
            if (house.name == houseName)
                return house;
        return null;
    }

    public Bucket GetEmptyBucket()
    {
        List<Bucket> buckets = GetFromInventory<Bucket>();
        foreach (Bucket bucket in buckets)
            if (!bucket.Filled)
                return bucket;
        return null;
    }

    public Bucket GetFilledBucket<T>() where T : Item
    {
        List<Bucket> buckets = GetFromInventory<Bucket>();
        buckets.RemoveAll(x => !x.Filled);
        foreach (Bucket bucket in buckets)
            if (bucket.item as T != null)
                return bucket;
        return null;
    }
    #endregion

    #region Override Functions
    protected override void Execute()
    {
        execute = StartCoroutine(_Execute());
    }
    
    private Coroutine execute;
    private IEnumerator _Execute()
    {
        if(debug)
            Debug.Log("STARTED: " + name + " " + curAction.name + " " + TimeManager.time);

        if (curAction.IsExecutable())
        {
            int frame = 0;
            Transform target = curAction.PosTrans();

            while (!curAction.InRange(target))
            {
                if (curAction.GetRemainingLinks().Count > 0)
                {
                    Stop();
                    yield break;
                }

                frame++;
                if (frame % settings.movementFramesUntilNewCheck == 0)
                    if (curAction.IsExecutable())
                        target = curAction.PosTrans();
                    else
                    {
                        Stop();
                        yield break;
                    }

                curAction.WhileMoving();
                movement.Follow(target);
                yield return null;
            }
            
            if (curAction.IsExecutable())
            {
                if (!curAction.autoMovement)
                    movement.Stop();
                base.Execute();
                yield break;
            }
        }

        Stop();
    }

    private void Stop()
    {
        Debug.Log("CANCELLED: " + name + " " + curAction.name + " " + TimeManager.time);
        movement.Stop();
        base.Cancel();
        NewEvent();
    }
    #endregion

    public override void Cancel()
    {
        if (execute != null)
            StopCoroutine(execute);
        movement.Stop();
        base.Cancel();
    }
}
