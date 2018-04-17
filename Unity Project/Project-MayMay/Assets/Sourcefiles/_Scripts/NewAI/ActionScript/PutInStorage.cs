using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "PutInStorage", menuName = "Actions/PutInStorage", order = 1)]
public class PutInStorage : RootAction
{
    private Preparedness Preparedness
    {
        get
        {
            return Stat<Preparedness>();
        }
    }

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override int GetReturnValue()
    {
        return Max;
    }

    protected override bool ExecutableCheck()
    {
        return GetStorage() != null;
    }

    public override void Execute()
    {
        int i = Preparedness.InventoryBufferFood();
        if(i > 0)
            PutItemsInStorage<Food>(i);
        i = Preparedness.InventoryBufferDrink();
        if (i > 0)
            PutItemsInStorage<Water>(i);

        Complete();
    }

    private void PutItemsInStorage<T>(int amount) where T : Consumable
    {
        List<T> consumables = ai.GetFromInventory<T>();
        Storage storage = GetStorage();
        Consumable consumable;

        for (int i = 0; i < amount; i++)
        {
            consumable = consumables.First();
            storage.AddToInventory(consumable);
            consumables.RemoveAt(0);
            ai.inventory.Remove(consumable);
        }
    }

    private Storage GetStorage()
    {
        List<Storage> storages = ai.GetFromInteractables<Storage>();
        storages.RemoveAll(x => x.Full);
        return storages.SortByClosest(ai.Pos).First();
    }

    public override Transform PosTrans()
    {
        return GetStorage().transform;
    }
}
