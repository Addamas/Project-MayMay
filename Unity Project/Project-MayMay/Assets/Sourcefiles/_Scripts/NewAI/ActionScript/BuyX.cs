using Jext;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectShortcuts;

[CreateAssetMenu(fileName = "BuyItem", menuName = "Actions/Convince/BuyItem", order = 1)]
public class BuyX : ConverseNormal {

    [SerializeField]
    protected Item item;
    [SerializeField]
    protected List<Link> links;

    public override List<Link> GetReturnValue()
    {
        return links;
    }

    protected override bool AvailableCheck(Memory.Other other)
    {
        ShopKeeping shopkeeping = other.character.GetAction<ShopKeeping>();

        if (shopkeeping == null)
            return false;

        if (!shopkeeping.Open)
            return false;

        if (!ContainsItem(shopkeeping, item))
            return false;

        return base.AvailableCheck(other);
    }

    private bool ContainsItem(ShopKeeping shopkeeping, Item item)
    {
        List<Item> items = shopkeeping.Inventory;

        bool fit = false;
        foreach (Item otherItem in items)
            if (otherItem.GetType() == item.GetType())
            {
                fit = true;
                break;
            }
        return fit;
    }

    protected override bool ExecutableCheck()
    {
        return ContainsItem(GetOther().character.GetAction<ShopKeeping>(), item);
    }

    protected override void WhenCompleted(Memory.Other other)
    {
        try
        {
            ShopKeeping shopkeeper = other.character.GetAction<ShopKeeping>();
            shopkeeper.Sell(item, ai);
        }
        catch
        {
            base.WhenCompleted(other);
        }
    }

    protected override Social.Conversation PickConversation(Memory.Other other)
    {
        return other.character.GetStat<Social>().GetConversation(Social.ConversationType.Buying);
    }

    protected override Memory.Other GetOther()
    {
        List<Memory.Other> shopKeepers = new List<Memory.Other>();

        foreach (Memory.Other other in ai.memory.relatives)
            if (AvailableCheck(other))
                shopKeepers.Add(other);

        return Shortcuts.GetClosest(ref shopKeepers, ai.Pos);
    }
}