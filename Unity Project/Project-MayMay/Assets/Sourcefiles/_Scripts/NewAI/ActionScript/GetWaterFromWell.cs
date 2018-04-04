using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "GetWaterFromWell", menuName = "Actions/GetWaterFromWell", order = 1)]
public class GetWaterFromWell : NormalActionMulFrameable
{
    [SerializeField]
    private float duration;

    private Water Water
    {
        get
        {
            return Well.water;
        }
    }

    private Well Well
    {
        get
        {
            return ai.GetFromInteractables<Well>().GetClosest(ai.Pos);
        }
    }

    public override List<Link> GetRemainingLinks()
    {
        List<Link> ret = new List<Link>();
        if (ai.GetEmptyBucket() == null)
            ret.Add(Link.hasBucket);
        return ret;
    }

    public override List<Link> GetReturnValue()
    {
        return new List<Link>() {Link.hasFilledBucket };
    }

    public override IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(duration);

        if (ai.GetEmptyBucket() == null) {
            ai.ForceNewEvent();
            yield break;
        }

        ai.GetEmptyBucket().item = Well.water;

        Complete();
    }

    protected override bool ExecutableCheck()
    {
        return Well != null;
    }

    public override Transform PosTrans()
    {
        return Well.transform;
    }
}
