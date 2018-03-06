using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ownable : MonoBehaviour {

    [NonSerialized]
    public List<Character> owner = new List<Character>();
    [SerializeField]
    protected bool isPublic;

    public bool HasOwner
    {
        get
        {
            return owner.Count > 0;
        }
    }
}
