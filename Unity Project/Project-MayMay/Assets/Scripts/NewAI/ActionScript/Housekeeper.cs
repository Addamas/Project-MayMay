using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HouseKeeping", menuName = "Actions/HouseKeeping", order = 1)]
public class Housekeeper : RootActionMulFrameable
{
    [SerializeField]
    private string houseName;

    protected House GetHouse()
    {
        return ai.GetHouse(houseName);
    }

    public override List<Link> GetRemainingLinks()
    {
        List<Link> ret = new List<Link>();
        if (!GetHouse().Open)
            ret.Add(Link.OpenedHouse);
        return ret;
    }

    public override int GetReturnValue()
    {
        return Max;
    }

    public override IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(2); //temporarily
        Complete();
    }

    public override Transform PosTrans()
    {
        return GetHouse().transform;
    }

    protected override bool ExecutableCheck()
    {
        GetHouse();
        return true;
    }
}
