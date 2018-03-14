using System;
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

    public override void Init()
    {
        memory = character.memory;
        Methods.MakeCloneSOList(ref senses);
        senses.ForEach(x => x.Init(this));
        StartCoroutine(CheckSurrounding());
    }

    public List<Memory.Other> GetSurrounding()
    {
        List<Memory.Other> characters = new List<Memory.Other>();
        character.memory.relatives.ForEach(x => characters.Add(x));

        characters.RemoveAll(x => !InRange(x.character.transform));

        //normally check if hearable / seeable

        return characters;
    }

    //repeat intern call
    private IEnumerator CheckSurrounding()
    {
        while(true)
        {
            List<Memory.Other> surrounding = GetSurrounding();
            surrounding.ForEach(x => memory.AddMemory(x, x.character.curAction));
            senses.ForEach(x => x.TryExecute(surrounding));
            yield return new WaitForSeconds(settings.frequency);
        }
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
