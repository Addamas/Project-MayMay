using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ownable : MonoBehaviour {

    [NonSerialized]
    public List<Character> owners = new List<Character>();
    [SerializeField]
    protected bool isPublic;

    public bool HasOwner
    {
        get
        {
            return owners.Count > 0;
        }
    }

    public virtual void Init() {

    }
}
