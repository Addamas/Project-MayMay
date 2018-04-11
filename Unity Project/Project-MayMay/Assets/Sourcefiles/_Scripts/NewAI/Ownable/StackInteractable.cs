using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackInteractable : Interactable {

    public Item item;

    public bool Filled
    {
        get
        {
            return item != null;
        }
    }

    public override void Interact(Character character)
    {
        
        base.Interact(character);
    }

    public void EmptyStack()
    {
        item = null;
    }
}
