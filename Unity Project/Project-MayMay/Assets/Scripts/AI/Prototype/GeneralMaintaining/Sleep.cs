using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sleep", menuName = "Actions/Sleep", order = 1)]
public class Sleep : SimpleQuest
{
    public override Vector3 Pos()
    {
        return GetInteractableX.SPGetAllX<Bed>(ai)[0].transform.position;
    }
}
