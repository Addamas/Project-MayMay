using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Senses : CharacterExtension
{
    [SerializeField]
    private float frequency, spotDistance;
    private Memory memory;

    public override void Init()
    {
        memory = character.memory;
        StartCoroutine(CheckSurrounding());
    }

    public List<Character> GetSurrounding()
    {
        return new List<Character>();
    }

    //repeat intern call
    private IEnumerator CheckSurrounding()
    {
        while(true)
        {
            List<Character> surrounding = GetSurrounding();
            surrounding.ForEach(x => memory.AddMemory(memory.GetInfoCharacter(x), x.curAction));
            yield return new WaitForSeconds(frequency);
        }
    }
}
