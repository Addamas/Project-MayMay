using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SensePerson", menuName = "Sense/SensePerson", order = 1)]
public class SensePerson : Sense {

    protected SearchX searchAction;

    public override void Init(Senses senses)
    {
        base.Init(senses);
        searchAction = character.GetAction<SearchX>();
    }

    public override bool ShouldExecute(List<Memory.Other> surrounding)
    {
        return searchAction.Searching;
    }

    public override void Execute(List<Memory.Other> surrounding)
    {
        Character target = searchAction.target;
        foreach (Memory.Other other in surrounding)
            if (other.character == target)
            {               
                Debug.Log("Found " + target.name);
                searchAction.Complete();
                return;
            }
    }
}
