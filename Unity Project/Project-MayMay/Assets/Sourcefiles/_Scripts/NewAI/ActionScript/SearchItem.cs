using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SearchItem", menuName = "Actions/SearchItem", order = 1)]
public class SearchItem : NormalActionMulFrameable
{
    [NonSerialized]
    public Type target;

    private Item Target {
        get
        {
            foreach (Item item in ai.ownedItems)
                if (item.GetType() == target)
                    return item;
            return null;
        }
    }

    public bool CanFind(Type type)
    {
        foreach (Item item in ai.ownedItems)
            if (item.GetType() == type)
                return true;
        return false;
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
        return new List<Link>() {Link.HasItem };
    }

    public override IEnumerator LifeTime()
    {
        Target.PutInInventory(ai);
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
