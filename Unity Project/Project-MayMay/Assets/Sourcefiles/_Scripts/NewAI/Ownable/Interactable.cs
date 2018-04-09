using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Ownable {

    public virtual void Interact(Character character)
    {

    }

    public void Init()
    {
        if(isPublic)
            foreach(Character character in GameManager.characters)
            {
                character.interactables.Add(this);
                owners.Add(character);
            }
    }
}
