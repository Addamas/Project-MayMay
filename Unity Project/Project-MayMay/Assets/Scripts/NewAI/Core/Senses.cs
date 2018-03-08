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

    public List<Character> GetSurrounding()
    {
        List<Character> characters = new List<Character>();
        GameManager.characters.ForEach(x => characters.Add(x));
        characters.Remove(character);

        characters.RemoveAll(x => Vector3.Distance(character.Pos, x.Pos) > settings.spotDistance);

        //normally check if hearable / seeable

        return characters;
    }

    //repeat intern call
    private IEnumerator CheckSurrounding()
    {
        while(true)
        {
            List<Character> surrounding = GetSurrounding();
            surrounding.ForEach(x => memory.AddMemory(memory.GetInfoCharacter(x), x.curAction));
            yield return new WaitForSeconds(settings.frequency);
        }
    }

    public bool TrySpot(Character character)
    {
        List<Character> surrounding = GetSurrounding();
        if (surrounding.Contains(character))
        {
            memory.AddMemory(memory.GetInfoCharacter(character), character.curAction);
            return true;
        }
        return false;
    }
}
