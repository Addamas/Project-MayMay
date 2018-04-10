using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shop : House {

    public List<Item> items = new List<Item>();

    public virtual void PlaceItemInShop(Item item)
    {
        items.Add(item);
    }

    public virtual void Sell(Item item, Character buyer)
    {
        foreach(Item sellable in items)
            if(sellable.GetType() == item.GetType())
            {
                buyer.ownedItems.Add(sellable);
                buyer.GetAction<SearchItem>().target = sellable.GetType();
                items.Remove(sellable);
                return;
            }
    }
}
