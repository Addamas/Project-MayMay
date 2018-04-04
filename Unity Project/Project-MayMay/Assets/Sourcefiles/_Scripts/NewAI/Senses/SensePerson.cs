using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SensePerson", menuName = "Sense/SensePerson", order = 1)]
public class SensePerson : Sense {

    private SearchStat searchStat;

    public override void Init(Senses senses)
    {
        base.Init(senses);
        searchStat = character.GetStat<SearchStat>();
    }

    public override void Execute(List<Memory.Other> surrounding)
    {
        Character target = searchStat.target;
        foreach (Memory.Other other in surrounding)
            if (other.character == target)
            {               
                Debug.Log("Found " + target.name);
                searchStat.target = null;
                character.ForceNewEvent();
                return;
            }
    }

    public override bool ShouldExecute(List<Memory.Other> surrounding)
    {
        return searchStat.Searching;
    }
}
