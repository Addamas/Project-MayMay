using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "Consume", menuName = "Actions/Consume", order = 1)]
public class Consume : SimpleRootAction
{
    public override void Cancel()
    {
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {
        Consumable consumable = GetConsumable();
        consumable.Consume(AI<Character>());
        if (GetConsumables().Count == 0)
            AI<Character>().filledRequirements.Remove(Jai.Requirement.hasFood);
        Complete();
    }

    public override float GetEstimatedTimeRequired()
    {
        return 0;
    }

    public override int GetReturnValue()
    {
        if(GetConsumables().Count == 0)
            return 0;
        return GetConsumable().GetFillAmount();
    }

    public override bool IsInRange()
    {
        return true;
    }

    public override Vector3 Pos()
    {
        throw new System.NotImplementedException();
    }

    private Consumable GetConsumable()
    {
        List<Consumable> consumables = GetConsumables();
        consumables.Sort();
        return consumables[0];
    }

    private List<Consumable> GetConsumables()
    {
        return AI<Character>().ownedItems.GetTypeFromListAsU<Item, Consumable>();
    }
}
