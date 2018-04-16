using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "GoPeepee", menuName = "Actions/GoPeepee", order = 1)]
public class GoPeepee : RootActionMulFrameable
{
    [SerializeField]
    private float maxDuration;
    public float Duration
    {
        get
        {
            return maxDuration * ((float)(Max - stat.GetValue()) / 100);
        }
    }

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override int GetReturnValue()
    {
        return Max;
    }

    public override IEnumerator LifeTime()
    {
        GetWC().Interact(ai);
        yield return new WaitForSeconds(Duration);
        Complete();
    }

    public override Transform PosTrans()
    {
        return GetWC().transform;
    }

    protected override bool ExecutableCheck()
    {
        try
        {
            GetWC();
            return true;
        }
        catch
        {
            return false;
        }
    }

    private WC GetWC()
    {
        return ai.GetFromInteractables<WC>()[0];
    }

    public override float GetEstimatedTimeRequired()
    {
        return base.GetEstimatedTimeRequired() + Duration;
    }
}
