﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

public class Senses : CharacterExtension
{
    public SenseObject settings;
    [SerializeField]
    private List<Sense> senses = new List<Sense>();
    private Memory memory;
    [SerializeField]
    private int surroundingCap;

    public override void Init()
    {
        memory = character.memory;
        Methods.MakeCloneSOList(ref senses);
        senses.ForEach(x => x.Init(this));
    }

    public List<Memory.Other> GetSurrounding()
    {
        List<Memory.Other> characters = new List<Memory.Other>();

        foreach (Memory.Other character in character.memory.relatives)
            if (InRange(character.character.transform))
            {
                characters.Add(character);
                if (characters.Count >= surroundingCap)
                    break;
            }

        //normally check if hearable / seeable

        return characters;
    }

    private List<Memory.Other> surrounding;
    public void CheckSurrounding()
    {
        surrounding = GetSurrounding();
        surrounding.ForEach(x => memory.AddMemory(x, x.character.curAction));
        senses.ForEach(x => x.TryExecute(surrounding));
    }

    public bool TrySpot(Character character)
    {
        List<Memory.Other> surrounding = GetSurrounding();
        Memory.Other _character = memory.GetInfoCharacter(character);
        if (surrounding.Contains(_character))
        {
            memory.AddMemory(memory.GetInfoCharacter(character), character.curAction);
            return true;
        }
        return false;
    }

    public bool TrySpot(Interactable interactable)
    {
        return InRange(interactable.transform);
    }

    private bool InRange(Transform trans)
    {
        return Vector3.Distance(character.Pos, trans.position) < settings.spotDistance;
    }
}

public abstract class Sense : ScriptableObject
{
    [NonSerialized]
    public Senses senses;
    [NonSerialized]
    public Character character;

    public virtual void Init(Senses senses)
    {
        this.senses = senses;
        character = senses.character;
    }

    public void TryExecute(List<Memory.Other> surrounding)
    {
        if (ShouldExecute(surrounding))
            Execute(surrounding);
    }
    public abstract bool ShouldExecute(List<Memory.Other> surrounding);
    public abstract void Execute(List<Memory.Other> surrounding);
}
