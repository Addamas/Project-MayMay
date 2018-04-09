using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SensePerson", menuName = "Sense/SensePerson", order = 1)]
public class SensePerson : Sense {

    protected SearchCharacterStat searchStat;

    public override void Init(Senses senses)
    {
        base.Init(senses);
        searchStat = character.GetStat<SearchCharacterStat>();
    }

    public override bool ShouldExecute(List<Memory.Other> surrounding)
    {
        return searchStat.Searching;
    }

    public override void Execute(List<Memory.Other> surrounding)
    {
        Character target = searchStat.target;
        foreach (Memory.Other other in surrounding)
            if (other.character == target)
            {               
                Debug.Log("Found " + target.name);
                searchStat.FoundTarget();
                character.ForceNewEvent();
                return;
            }
    }
}
