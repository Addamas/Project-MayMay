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
            ret.Add(Link.HasItem);
        return ret;
    }

    public override List<Link> GetReturnValue()
    {
        return new List<Link>() {Link.HasFilledBucket };
    }

    public override IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(duration);

        if (ai.GetEmptyBucket() == null) {
            ai.ForceNewEvent();
            yield break;
        }

        Well well = Well;
        well.Interact(ai);

        Complete();
    }

    protected override bool ExecutableCheck()
    {
        if (GetRemainingLinks().Count > 0)
            return ai.GetAction<SearchItem>().CanFind(typeof(Bucket));
        return Well != null;
    }

    public override Transform PosTrans()
    {
        return Well.transform;
    }

    public override void Prepare()
    {
        ai.GetAction<SearchItem>().target = typeof(Bucket);
        base.Prepare();
    }
}
