using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterExtension : MonoBehaviour {

    protected Character character;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
    }
}
