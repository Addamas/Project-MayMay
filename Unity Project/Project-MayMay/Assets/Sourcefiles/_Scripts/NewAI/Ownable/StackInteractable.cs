using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackInteractable : Interactable {

    public Item item;

    [NonSerialized]
    public Type type;

    public bool Filled
    {
        get
        {
            return item != null;
        }
    }

    public override void Interact(Character character)
    {
        if (Filled)
        {
            item.PutInInventory(character);
            EmptyStack();
        }
        else try
            {
                Item item = character.GetFromInventory(type);
                item.PutInWorld(character, transform.position);
                this.item = item;
            }
            catch { }

        base.Interact(character);
    }

    public void EmptyStack()
    {
        item = null;
    }
}
