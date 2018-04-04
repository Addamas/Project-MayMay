using Jext;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectShortcuts;

[CreateAssetMenu(fileName = "BuyItem", menuName = "Actions/Convince/BuyItem", order = 1)]
public class BuyX : ConverseNormal {

    [SerializeField]
    protected List<Item> items;
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

        foreach (Item item in items)
            if (!shopkeeping.inventory.Contains(item))
                return false;

        return base.AvailableCheck(other);
    }

    protected override bool ExecutableCheck()
    {
        return GetOther().character.GetAction<ShopKeeping>() != null;
    }

    protected override Social.Conversation PickConversation(Memory.Other other)
    {
        return other.character.GetStat<Social>().GetConversation(Social.ConversationType.Buying);
    }

    protected override void WhenCompleted()
    {
        ai.inventory.AddList(items, true);
        base.WhenCompleted();
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