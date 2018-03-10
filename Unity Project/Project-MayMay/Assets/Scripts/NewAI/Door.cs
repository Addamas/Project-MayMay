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
            Close();
        else
            Open();
    }

    protected void Open()
    {
        open = true;
    }

    protected void Close()
    {
        open = false;
    }
}
