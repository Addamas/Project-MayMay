using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Ownable {

    public virtual void Interact(Character character)
    {

    }

    public void Init()
    {
        if (isPublic)
            GameManager.characters.ForEach(x => x.interactables.Add(this));
    }
}
