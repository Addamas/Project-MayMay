﻿using Jext;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectShortcuts;

[CreateAssetMenu(fileName = "BuyItem", menuName = "Actions/Convince/BuyItem", order = 1)]
public class BuyX : ConverseNormal {

    [SerializeField]
    protected Item item;
    [SerializeField]
    protected int amount = 1;
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

        if (!(ai.curAction == this ? shopkeeping.ShouldBeOpen : shopkeeping.Open))
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

    public override void Execute()
    {
        if(!GetOther().character.GetAction<ShopKeeping>().Open)
        {
            ai.ForceNewEvent();
            return;
        }
        base.Execute();
    }

    protected override void WhenCompleted(Memory.Other other)
    {
        ShopKeeping shopkeeper = other.character.GetAction<ShopKeeping>();

        for (int i = 0; i < amount; i++)
            try
            {
                shopkeeper.Sell(item, ai);
            }
            catch
            {
                break;
            }

        base.WhenCompleted(other);
    }

    protected override Social.Conversation PickConversation(Memory.Other other)
    {
        try
        {
            return other.GetConversation(Social.ConversationType.Buying);
        }
        catch
        {
            return Social.GetConversation(Social.ConversationType.Buying);
        }
    }

    protected override Memory.Other GetOther()
    {
        List<Memory.Other> shopKeepers = new List<Memory.Other>();

        foreach (Memory.Other other in ai.memory.relatives)
            if (AvailableCheck(other))
                shopKeepers.Add(other);

        return Shortcuts.GetClosest(ref shopKeepers, ai.Pos);
    }

    public override Transform PosTrans()
    {
        try
        {
            return base.PosTrans().GetComponent<Character>().GetFromInteractables<Shop>().First().transform;
        }
        catch
        {
            return base.PosTrans();
        }
    }
}