using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Jext;

[CreateAssetMenu(fileName = "SenseDoors", menuName = "Sense/SenseDoors", order = 1)]
public class SenseDoors : Sense
{
    private HouseManaging stat;

    public override void Init(Senses senses)
    {
        base.Init(senses);
        stat = character.GetStat(typeof(HouseManaging)) as HouseManaging;
    }

    private List<House> UnwantedUnlockHouses
    {
        get
        {
            List<House> ownedHouses = character.GetFromInteractables<House>();
            ownedHouses.RemoveAll(x => !x.UnwantedUnlock);
            return ownedHouses;
        }
    }

    public override void Execute(List<Memory.Other> surrounding)
    {
        //check if notices that doors have been closed
        int count = stat.unlockedDoors.Count - 1;
        for (int i = count; i >= 0; i--)
            if (!stat.unlockedDoors[i].IsOpen)
                if (senses.TrySpot(stat.unlockedDoors[i]))
                    stat.unlockedDoors.RemoveAt(i);

        //doesnt need to check for new since there are still closed doors
        if(stat.unlockedDoors.Count > 0)
        {
            character.NewEvent();
            return;
        }

        List<House> ownedHouses = UnwantedUnlockHouses;
        List<Door> unwantedOpenDoors = new List<Door>();
        foreach (House house in ownedHouses)
            unwantedOpenDoors.AddList(house.UnlockedDoors, false);

        unwantedOpenDoors.RemoveAll(x => !senses.TrySpot(x));
        stat.unlockedDoors.AddList(unwantedOpenDoors, false);

        if(stat.unlockedDoors.Count > 0)
            character.NewEvent();
    }

    public override bool ShouldExecute(List<Memory.Other> surrounding)
    {
        return UnwantedUnlockHouses.Count > 0;
    }
}
