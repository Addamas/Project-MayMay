using Jext;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shopkeeping", menuName = "Jobs/Shopkeeping", order = 1)]
public class ShopKeeping : RootActionMulFrameable
{
    public List<Item> inventory = new List<Item>();

    public bool Open
    {
        get
        {
            if (GetRemainingLinks().Count > 0)
                return false;
            return InRange();
        }
    }

    protected Shop Shop
    {
        get
        {
            return ai.GetFromInteractables<Shop>().First();
        }
    }

    public override List<Link> GetRemainingLinks()
    {
        List<Link> ret = new List<Link>();
        if (!Shop.Open) //when opening the backdoor and closing front door things can go wierd
            ret.Add(Link.OpenedShop);
        return ret;
    }

    public override int GetReturnValue()
    {
        return Max;
    }

    public override float GetEstimatedTimeRequired()
    {
        if(!InRange())
            return base.GetEstimatedTimeRequired();
        return TooLong;
    }

    public override IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(ai.senses.settings.frequency);
        Complete();
    }

    public override Transform PosTrans()
    {
        return Shop.transform;
    }

    protected override bool ExecutableCheck()
    {
        return Shop != null;
    }
}
