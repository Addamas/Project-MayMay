using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : SimpleConsumable
{
    public override void Consume(Character character)
    {
        foreach (Stat stat in character.stats)
            if (stat as Hunger != null)
            {
                stat.AddValue(GetFillAmount());
                base.Consume(character);
                return;
            }
    }
}