using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Jext;

public class Shop : House {

    public List<ItemStack> items = new List<ItemStack>(), 
        storage = new List<ItemStack>(), 
        provisions = new List<ItemStack>();

    public override void Init()
    {
        base.Init();

        InitItemStack(items);
        InitItemStack(storage);
    }

    private void InitItemStack(List<ItemStack> Istack)
    {
        foreach (ItemStack itemStack in Istack)
            foreach (StackInteractable interactable in itemStack.stack)
                interactable.type = itemStack.Type;
    }

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
        public StackInteractable[] stack;
    }

    public StackInteractable GetRestockable()
    {
        foreach (ItemStack itemstack in items)
            foreach (StackInteractable stack in itemstack.stack)
                if (!stack.Filled)
                    return stack;
        return null;
    }

    public StackInteractable GetStack(Item item, bool filled)
    {
        Type type = item.GetType();
        foreach (ItemStack itemStack in items)
            if (itemStack.Type == type)
                foreach (StackInteractable stack in itemStack.stack)
                    if (stack.Filled == filled)
                        if(!stack.sold)
                            return stack;

        //It needs a return variable, even a wrong one
        return items.First().stack.First();
    }

    public virtual void PlaceItemInShop(Item item)
    {
        StackInteractable stack = GetStack(item, false);
        stack.item = item;
    }

    public virtual void Sell(Item item, Character buyer)
    {
        StackInteractable stack = GetStack(item, true);
        stack.sold = true;

        Item sellable = stack.item;

        buyer.ownedItems.Add(sellable);
        sellable.owners.Clear();
        sellable.owners.Add(buyer);

        Interact interact = buyer.GetAction<Interact>();
        interact.target = stack;
        buyer.ForceAction(interact);
    }
}
