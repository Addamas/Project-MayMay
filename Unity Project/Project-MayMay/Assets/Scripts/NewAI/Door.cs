using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable {

    [SerializeField]
    private bool open;
    public bool IsOpen
    {
        get
        {
            return open;
        }
    }

    public override void Interact(Character character)
    {
        if (open)
            Close(character);
        else
            Open(character);
    }

    protected virtual void Open(Character character)
    {
        open = true;
    }

    protected virtual void Close(Character character)
    {
        open = false;
    }
}
