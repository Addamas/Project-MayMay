using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "Shopkeeper", menuName = "Actions/Shopkeeper", order = 1)]
public class Shopkeeper : SimpleRootAction
{
    private Shop shop;
    public bool open;

    public override void Init(Jai ai, Stat stat)
    {
        base.Init(ai, stat);
        shop = GetInteractableX.SPGetAllX<Shop>(AI<Character>())[0];
    }

    public override void Cancel()
    {
        open = false;
    }

    public override void Execute()
    {
        open = true;
    }

    public override float GetEstimatedTimeRequired()
    {
        return 0;
    }

    public override int GetReturnValue()
    {
        return 1;
    }

    public override bool IsInRange()
    {
        return Dis() < AI<Character>().interactDistance;
    }

    public override Vector3 Pos()
    {
        return shop.transform.position;
    }

    public override List<Jai.Requirement> GetRequirements()
    {
        List<Jai.Requirement> requirements = Methods.Clone(this.requirements);
        requirements.Add(STAT<Quest>().IsTime() ? Jai.Requirement.openShop : Jai.Requirement.closedShop);
        return requirements;
    }
}


public class ShopkeepAction : SimpleNormalAction
{
    protected Shop shop;
    public bool open;

    public override void Init(Jai ai)
    {
        base.Init(ai);
        shop = GetInteractableX.SPGetAllX<Shop>(AI<Character>())[0];
    }

    public override void Cancel()
    {
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }

    public override float GetEstimatedTimeRequired()
    {
        throw new System.NotImplementedException();
    }

    public override bool IsInRange()
    {
        throw new System.NotImplementedException();
    }

    public override Vector3 Pos()
    {
        throw new System.NotImplementedException();
    }
}