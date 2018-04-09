using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseItems : Sense {

    protected SearchItemStat searchStat;
    [SerializeField]
    protected bool caresAboutOwnership = true;

    public override void Init(Senses senses)
    {
        base.Init(senses);
        searchStat = character.GetStat<SearchItemStat>();
    }

    public override bool ShouldExecute(List<Memory.Other> surrounding)
    {
        return searchStat.Searching;
    }

    public override void Execute(List<Memory.Other> surrounding)
    {
        List<Item> surroundingItems = new List<Item>();
        Type target = searchStat.target;

        foreach (Item other in surroundingItems)
            if (other.GetType() == target)
            {
                if (!other.owners.Contains(character))
                    continue;
                Debug.Log("Found " + other.name);
                searchStat.target = null;
                character.ForceNewEvent();
                return;
            }
    }
}
