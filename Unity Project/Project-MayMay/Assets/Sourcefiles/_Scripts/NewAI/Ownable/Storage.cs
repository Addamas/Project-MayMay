using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Interactable {

    [SerializeField]
    private int capacity;
    public List<Item> inventory = new List<Item>();

    public bool Full
    {
        get
        {
            return inventory.Count >= capacity;
        }
    }

    public void AddToInventory(Item item)
    {
        inventory.Add(item);
    }

    public Item GetFromInventory(Type type)
    {
        foreach (Item item in inventory)
            if (item.GetType() == type)
            {
                Item ret = item;
                inventory.Remove(item);
                return ret;
            }
        return null;
    }
}
