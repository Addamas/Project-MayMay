using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "GetFood", menuName = "Actions/GetFood", order = 1)]
public class GetFood : GetInteractableX
{
    protected List<GrantConsumables> locations;

    public override void Init(Jai ai)
    {
        base.Init(ai);
        locations = SGetAllX<GrantConsumables>(ai as Character);
    }

    public override void Execute()
    {
        ai.ownedItems.Add(GetX<GrantConsumables>().GetConsumable());
        Complete();
    }

    public override Vector3 Pos()
    {
        return GetX<Well>().transform.position;
    }

    protected override T GetX<T>()
    {
<<<<<<< HEAD
=======
        locations.SortByClosest(ai.transform.position);
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d
        return locations[0] as T;
    }
}
