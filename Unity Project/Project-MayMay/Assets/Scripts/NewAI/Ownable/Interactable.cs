using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : Ownable {

    protected virtual void OnEnable()
    {
        if (isPublic)
            GameManager.characters.ForEach(x => x.interactables.Add(this));
    }
}
