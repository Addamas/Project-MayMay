using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consume", menuName = "Actions/Consume", order = 1)]
public class Consume : RootAction
{
    [SerializeField]
    private string peepeeStatName;

    private bool HasFood
    {
        get
        {
            try
            {
                GetConsumable();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    private Consumable GetConsumable()
    {
        List<Consumable> ret = ai.GetFromInventory<Consumable>();
        ret.Sort();
        return ret[0];
    }

    public override List<Link> GetRemainingLinks()
    {
        List<Link> ret = new List<Link>();
        if (!HasFood)
            ret.Add(Link.HasFood);
        return ret;
    }

    public override int GetReturnValue()
    {
        try
        {
            return GetConsumable().value;
        }
        catch
        {
            return 0;
        }
    }

    public override bool InRange()
    {
        return true;
    }

    protected override bool ExecutableCheck()
    {
        return true;
    }

    public override void Execute()
    {
        Consumable consumable = GetConsumable();
        consumable.Consume(Stat<TickStat>());
        ai.inventory.Remove(consumable);

        ai.GetStat(peepeeStatName).AddValue(Mathf.RoundToInt(consumable.peePerValuePoint * consumable.value));

        Complete();
    }

    public override void Cancel()
    {
        
    }
}
