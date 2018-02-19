using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Consumable : Item, IComparable<Consumable> {
    
    public abstract int GetFillAmount();

    public int CompareTo(Consumable other)
    {
        return other.GetFillAmount() - GetFillAmount();
    }

    public virtual void Consume(Character character)
    {
        if(character.ownedItems.Contains(this))
            character.ownedItems.Remove(this);
    }
}

public abstract class SimpleConsumable : Consumable
{
    [SerializeField]
    private int fillAmount;

    public override int GetFillAmount()
    {
        return fillAmount;
    }
}
