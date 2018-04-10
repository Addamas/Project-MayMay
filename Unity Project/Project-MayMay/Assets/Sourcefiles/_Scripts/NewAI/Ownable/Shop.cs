using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Jext;

public class Shop : House {

    public List<ItemStack> items = new List<ItemStack>();

    [Serializable]
    public class ItemStack {
        public Item itemType;
        public Type Type
        {
            get
            {
                return itemType.GetType();
            }
        }
        public Stack[] stack;
    }

    [Serializable]
    public class Stack {
        public Item item;
        public Transform spot;

        public bool Filled
        {
            get
            {
                return item != null;
            }
        }

        public void EmptyStack()
        {
            item = null;
        }
    }

    public Stack GetStack(Item item, bool filled)
    {
        Type type = item.GetType();
        foreach (ItemStack itemStack in items)
            if (itemStack.Type == type)
                foreach (Stack stack in itemStack.stack)
                    if (stack.Filled == filled)
                        return stack;

        //It needs a return variable, even a wrong one
        return items.First().stack.First();
    }

    public virtual void PlaceItemInShop(Item item)
    {
        Stack stack = GetStack(item, false);
        stack.item = item;
    }

    public virtual void Sell(Item item, Character buyer)
    {
        Stack stack = GetStack(item, true);
        Item sellable = stack.item;    
        
        buyer.ownedItems.Add(sellable);
        sellable.owners.Clear();
        sellable.owners.Add(buyer);

        stack.EmptyStack();
        buyer.GetAction<SearchItem>().target = sellable.GetType();
    }
}
