using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopAssistant", menuName = "Jobs/ShopAssistant", order = 1)]
public class ShopAssistant : RootActionMulFrameable
{
    private ShopKeeping boss;
    private ShopKeeping Boss
    {
        get
        {
            if (boss == null)
                boss = Stat<QuestAssistant>().LeaderQuest.GetAction<ShopKeeping>();
            return boss;
        }
    }

    private Shop Shop
    {
        get
        {
            return Boss.Shop;
        }
    }

    public override List<Link> GetRemainingLinks()
    {
        List<Link> ret = new List<Link>();
        if (GetInteractable() != null)
            ret.Add(Link.Interacted);
        return ret;
    }

    private StackInteractable GetInteractable()
    {
        StackInteractable interactable = Shop.GetRestockable();
        if (interactable != null)
            return GetShopItemFromInventory(interactable.type).Count > 0 ? 
                interactable : GetStorageInteractable(interactable.type);
        return null;
    }

    private List<Item> GetShopItemFromInventory(Type type)
    {
        return ai.GetFromInventory(type, Boss.ai);
    }

    private StackInteractable GetStorageInteractable(Type type)
    {
        Shop shop = Shop;
        foreach (Shop.ItemStack stack in shop.storage)
            if (stack.Type == type)
                foreach (StackInteractable interactable in stack.stack)
                    if (interactable.Filled)
                        return interactable;
        return null;
    }

    public override int GetReturnValue()
    {
        return Min + 1;
    }

    public override IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(ai.senses.settings.frequency);
        Complete();
    }

    protected override bool ExecutableCheck()
    {
        return true;
    }

    public override Transform PosTrans()
    {
        return Shop.transform;
    }

    public override void Prepare()
    {
        StackInteractable stack = GetInteractable();
        ai.GetAction<Interact>().target = stack;
            
        base.Prepare();
    }
}
