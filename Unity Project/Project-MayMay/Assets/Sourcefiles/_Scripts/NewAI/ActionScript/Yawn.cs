using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Yawn", menuName = "Actions/Yawn", order = 1)]
public class Yawn : RootActionMulFrameable
{
    [SerializeField]
    private float duration;
    public float Duration
    {
        get
        {
            return duration;
        }
    }

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override int GetReturnValue()
    {
        return Min;
    }

    public override bool InRange()
    {
        return true;
    }

    public override float GetEstimatedTimeRequired()
    {
        return base.GetEstimatedTimeRequired() + Duration;
    }

    public override IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(duration);
        Complete();
    }

    protected override bool ExecutableCheck()
    {
        return true;
    }
}
