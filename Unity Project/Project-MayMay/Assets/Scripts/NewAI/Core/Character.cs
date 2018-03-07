using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

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

    #region Get Functions

    public T GetAction<T>() where T : Action
    {
        foreach (Action action in actions)
            if (action.GetType() is T)
                return action as T;
        foreach (Stat stat in stats)
            foreach (RootAction action in stat.rootActions)
                if (action.GetType() is T)
                    return action as T;
        return null;
    }

    public T GetStat<T>() where T : Stat
    {
        foreach (Stat stat in stats)
            if (stat.GetType() is T)
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

    public Area GetArea()
    {
        List<Area> districts = new List<Area>();
        GameManager.districts.ForEach(x => districts.Add(x));

        districts = districts.SortByClosest(Pos);
        return districts.First().GetClosestInDistrict(Pos);
    }

    #endregion

    #region Inventory & Available Interactables
    public List<Item> inventory = new List<Item>();
    public List<Interactable> interactables = new List<Interactable>();

    protected override void Awake()
    {
        #region Add Owner
        inventory.ForEach(x => x.owner.Add(this));
        interactables.ForEach(x => x.owner.Add(this));
        #endregion

        base.Awake();
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
    #endregion

    #region Override Functions
    
    protected override void Execute()
    {
        execute = StartCoroutine(_Execute());
    }
    
    private Coroutine execute;
    private IEnumerator _Execute()
    {
        while (curAction.IsExecutable())
        {
            if (curAction.GetRemainingLinks().Count > 0)
            {
                NewEvent();
                yield break;
            }
            
            if (curAction.InRange())
            {
                base.Execute();
                yield break;
            }

            movement.Follow(curAction.PosTrans());
            yield return null;
        }
    }

    protected override void StopMovement()
    {
        if (execute != null)
            StopCoroutine(execute);
        movement.Stop();
    }
    #endregion
}
