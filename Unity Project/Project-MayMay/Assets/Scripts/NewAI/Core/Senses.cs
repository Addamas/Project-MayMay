using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senses : CharacterExtension
{
    public SenseObject settings;
    private Memory memory;

    public override void Init()
    {
        memory = character.memory;
        StartCoroutine(CheckSurrounding());
    }

    public List<Memory.Other> GetSurrounding()
    {
        List<Memory.Other> characters = new List<Memory.Other>();
        character.memory.relatives.ForEach(x => characters.Add(x));

        characters.RemoveAll(x => Vector3.Distance(character.Pos, x.character.Pos) > settings.spotDistance);

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
}
