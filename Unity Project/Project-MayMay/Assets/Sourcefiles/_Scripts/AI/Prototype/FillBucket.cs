using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "FillBucket", menuName = "Actions/FillBucket", order = 1)]
public class FillBucket : GetWater
{
    public override void Complete()
    {
        ai.filledRequirements.Remove(Jai.Requirement.hasBucket);
        ai.filledRequirements.Add(Jai.Requirement.hasFilledBucket);

        base.Complete();
    }
}
