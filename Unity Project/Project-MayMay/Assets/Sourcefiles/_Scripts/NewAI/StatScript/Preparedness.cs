using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

//actions:
//put x in a storage
//"link" root action that filters required links

[CreateAssetMenu(fileName = "Preparedness", menuName = "Stats/Preparedness", order = 1)]
public class Preparedness : Stat
{
    [SerializeField]
    private int minValue;
    public int carriedFoodAmount, carriedDrinkAmount;

    public override void AddValue(int value)
    {
        
    }

    public override int GetValue()
    {
        return HasMoreThanEnoughInInventory() ? minValue : Max;
    }

    public bool HasMoreThanEnoughInInventory()
    {
        return ai.GetFromInventory<Food>().Count > carriedFoodAmount ||
            ai.GetFromInventory<Water>().Count > carriedDrinkAmount;
    }

    public int InventoryBufferFood()
    {
        return InventoryBufferItem<Food>(carriedFoodAmount);
    }

    public int InventoryBufferDrink()
    {
        return InventoryBufferItem<Water>(carriedDrinkAmount);
    }

    public int InventoryBufferItem<T>(int min) where T : Consumable
    {
        return ai.GetFromInventory<T>().Count - min;
    }
}
