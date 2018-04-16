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
                GetFood();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    private Food GetFood()
    {
        List<Food> ret = ai.GetFromInventory<Food>();
        ret.Sort();
        return ret[0];
    }

    public override List<Link> GetRemainingLinks()
    {
        List<Link> ret = new List<Link>();
        if (!HasFood)
            if (ai.GetAction<SearchItem>().CanFind(typeof(Food)))
                ret.Add(Link.HasItem);
            else
                ret.Add(Link.HasFood);
        return ret;
    }

    public override int GetReturnValue()
    {
        try
        {
            return GetFood().value;
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
        Food food = GetFood();

        food.Consume(Stat<TickStat>());
        ai.inventory.Remove(food);
        ai.GetStat(peepeeStatName).AddValue(Mathf.RoundToInt(food.peePerValuePoint * food.value));

        Complete();
    }

    public override void Cancel()
    {
        
    }

    public override void Prepare()
    {
        try
        {
            ai.GetAction<SearchItem>().target = typeof(Food);
        }
        catch
        {

        }
        try
        {
            ai.GetAction<GetFromStorage>().target = typeof(Food);
        }
        catch
        {

        }
        base.Prepare();
    }
}
