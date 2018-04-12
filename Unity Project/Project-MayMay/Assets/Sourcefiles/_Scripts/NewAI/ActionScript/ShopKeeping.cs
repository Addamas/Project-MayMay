using Jext;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shopkeeping", menuName = "Jobs/Shopkeeping", order = 1)]
public class ShopKeeping : RootActionMulFrameable
{
    public bool Open
    {
        get
        {
            if (GetRemainingLinks().Count > 0)
                return false;
            return InRange();
        }
    }

    private Shop shop;
    public Shop Shop
    {
        get
        {
            if (shop == null)
                shop = ai.GetFromInteractables<Shop>().First();
            return shop;
        }
    }

    public List<Item> Inventory
    {
        get
        {
            List<Item> ret = new List<Item>();
            foreach(Shop.ItemStack itemStack in Shop.items)
                foreach(StackInteractable stack in itemStack.stack)
                    if(stack.Filled)
                    {
                        ret.Add(stack.item);
                        break;
                    }
            return ret;
        }
    }

    public override void Init(GHOPE ai, Stat stat)
    {
        base.Init(ai, stat);
        Shop shop = this.ai.GetFromInteractables<Shop>().First();

        InitStacks(shop.items);
        InitStacks(shop.storage);
    }

    private void InitStacks(List<Shop.ItemStack> stack)
    {
        foreach (Shop.ItemStack itemStack in stack)
            foreach (StackInteractable interactable in itemStack.stack)
                if (interactable.Filled)
                    interactable.item.owners.Add(ai);
    }

    public virtual void Sell(Item item, Character buyer)
    {
        Shop.Sell(item, buyer);
    }

    public override List<Link> GetRemainingLinks()
    {
        List<Link> ret = new List<Link>();
        if (!Shop.Open)
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
