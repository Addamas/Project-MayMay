using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "GetWater", menuName = "Actions/GetWater", order = 1)]
public class GetWater : GetInteractableX
{
    protected List<Well> wells;

    public override void Init(Jai ai)
    {
        base.Init(ai);
        wells = GetAllX<Well>();
    }

    public override void Execute()
    {
        ai.ownedItems.Add(GetX<Well>().GetConsumable());
        Complete();
    }

    public override Vector3 Pos()
    {
        return GetX<Well>().transform.position;
    }

    protected override T GetX<T>()
    {
        return wells[0] as T;
    }
}
