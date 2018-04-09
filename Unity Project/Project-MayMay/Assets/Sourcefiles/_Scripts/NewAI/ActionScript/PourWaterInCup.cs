using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PourWaterInCup", menuName = "Actions/PourWaterInCup", order = 1)]
public class PourWaterInCup : NormalActionMulFrameable
{
    [SerializeField]
    private float duration;

    private Bucket Bucket
    {
        get
        {
            return ai.GetFilledBucket<Water>();
        }
    }

    public override List<Link> GetRemainingLinks()
    {
        List<Link> ret = new List<Link>();
        if (Bucket == null)
            ret.Add(Link.hasFilledBucket);
        return ret;
    }

    public override IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(duration);
        
        if(Bucket == null)
        {
            ai.ForceNewEvent();
            yield break;
        }

        ai.inventory.Add(Bucket.Empty());
        Complete();
    }

    protected override bool ExecutableCheck()
    {
        return true;
    }

    public override Transform PosTrans()
    {
        return base.PosTrans();
    }

    public override List<Link> GetReturnValue()
    {
        return new List<Link>() {Link.hasWater };
    }
}
