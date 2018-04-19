using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;
using System;

public class Character : GHOPE {

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
    public List<Item> inventory = new List<Item>(), ownedItems = new List<Item>();
    public List<Interactable> interactables = new List<Interactable>();

    public override void Init()
    {
        #region Add Owner
        inventory.ForEach(x => x.owners.Add(this));
        ownedItems.ForEach(x => x.owners.Add(this));
        interactables.ForEach(x => x.owners.Add(this));
        #endregion

        base.Init();
    }

    public List<T> GetFromInventory<T>() where T : Item
    {
        return inventory.GetTypeFromListAsU<Item, T>();
    }

    public List<T> GetFromOwnedItems<T>() where T : Item
    {
        return inventory.GetTypeFromListAsU<Item, T>();
    }

    public Item GetFromInventory(Type type)
    {
        foreach (Item item in inventory)
            if (item.GetType() == type)
                return item;
        return null;
    }

    public List<Item> GetFromInventory(Type type, Character owner)
    {
        List<Item> ret = new List<Item>();
        foreach (Item item in inventory)
            if (item.GetType() == type)
                if (item.owners.Contains(owner))
                    ret.Add(item);
        return ret;
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
    public override void Execute()
    {
        if (debug)
            Debug.Log("STARTED: " + name + " " + curAction.name + " " + TimeManager.time);

        GameManager.EnqueueMovement(this);
    }

    public void BaseExecute()
    {
        base.Execute();
    }
    
    public void StopMovement()
    {     
        movement.Stop();
    }
    #endregion

    public override void Cancel()
    {
        if (debug && curAction != null)
            Debug.Log("CANCELLED: " + name + " " + curAction.name + " " + TimeManager.time);
        GameManager.TryRemoveFromMovementQueue(this);
        movement.Stop();
        base.Cancel();
    }
}
