using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Ownable {

	public Item Pickup()
    {
        Destroy(transform);
        return this;
    }

    public void PutInInventory(Character character)
    {
        gameObject.SetActive(false);
        character.inventory.Add(this);
        foreach (Character other  in owners)
            character.ownedItems.Remove(this);
    }

    public void PutInWorld(Character character)
    {
        gameObject.SetActive(true);
        character.inventory.Remove(this);
        foreach (Character other in owners)
            character.ownedItems.Add(this);
    }

    public override void Init()
    {
        if (isPublic)
            foreach (Character character in GameManager.characters)
            {
                character.ownedItems.Add(this);
                owners.Add(character);
            }
        base.Init();
    }
}
