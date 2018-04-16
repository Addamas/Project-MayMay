using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "GetFromStorage", menuName = "Actions/GetFromStorage", order = 1)]
public class GetFromStorage : NormalActionMulFrameable
{
    [NonSerialized]
    public Type target;

    private Storage Target
    {
        get
        {
            return StorageContains(target);
        }
    }

    private Storage StorageContains(Type type)
    {
        List<Storage> storages = ai.GetFromInteractables<Storage>();
        foreach (Storage storage in storages)
            foreach (Item item in storage.inventory)
                if (item.GetType() == type)
                    return storage;
        return null;
    }

    public bool CanFind(Type type)
    {
        return Target != null;
    }

    public bool Searching
    {
        get
        {
            return target != null;
        }
    }

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override List<Link> GetReturnValue()
    {
        List<Link> ret = new List<Link>();

        if (StorageContains(typeof(Water)))
            ret.Add(Link.HasWater);
        if (StorageContains(typeof(Food)))
            ret.Add(Link.HasFood);

        return ret;
    }

    public override IEnumerator LifeTime()
    {
        ai.inventory.Add(Target.GetFromInventory(target));
        Complete();
        yield break;
    }

    protected override bool ExecutableCheck()
    {
        return true;
    }

    public override Transform PosTrans()
    {
        try
        {
            return Target.transform;
        }
        catch
        {
            return base.PosTrans();
        }
    }
}
