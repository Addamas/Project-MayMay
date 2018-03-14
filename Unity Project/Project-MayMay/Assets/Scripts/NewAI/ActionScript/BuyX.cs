using Jext;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuyItem", menuName = "Actions/Convince/BuyItem", order = 1)]
public class BuyX : ConverseNormal {

    [SerializeField]
    protected List<Item> items;
    [SerializeField]
    protected List<Link> links;
    [SerializeField]
    protected string shopKeepType;

    public override List<Link> GetReturnValue()
    {
        return links;
    }

    protected override bool AvailableCheck(Memory.Other other)
    {
        ShopKeeping shopkeeping = other.character.GetAction(shopKeepType) as ShopKeeping; 
        
        if (shopkeeping == null)
            return false;
        if (!shopkeeping.Open)
            return false;

        return base.AvailableCheck(other);
    }

    protected override bool ExecutableCheck()
    {
        return GetOther().character.GetAction(shopKeepType) != null;
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
}