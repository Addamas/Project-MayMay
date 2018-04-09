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
    }

    public void PutInWorld(Character character)
    {
        gameObject.SetActive(true);
        character.inventory.Remove(this);
    }

    public void Init()
    {
        if (isPublic)
            foreach (Character character in GameManager.characters)
            {
                character.ownedItems.Add(this);
                owners.Add(character);
            }
    }
}
