using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterExtension : MonoBehaviour {

    [NonSerialized]
    public Character character;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
    }

    public abstract void Init();
}
