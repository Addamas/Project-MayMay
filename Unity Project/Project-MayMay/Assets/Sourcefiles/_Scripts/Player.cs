using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Character interactingCharacter;    
    private bool Interacting
    {
        get
        {
            return interactingCharacter != null;
        }
    }
    public bool IsInteractingCharacter(Character character)
    {
        return character == interactingCharacter;
    }

    #region Start / Stop Interacting

    public void InteractWithCharacter(Character character)
    {
        if (Interacting)
            return;
        if (!character.PlayerInteract())
            return;
        interactingCharacter = character;
    }

    public void StopInteractingWithCharacter()
    {
        interactingCharacter = null;
    }

    #endregion
}
