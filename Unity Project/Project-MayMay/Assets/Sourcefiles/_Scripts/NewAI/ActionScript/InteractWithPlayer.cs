using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithPlayer : NormalActionMulFrameable
{
    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override List<Link> GetReturnValue()
    {
        return new List<Link>();
    }

    public override IEnumerator LifeTime()
    {
        while (GameManager.Player.IsInteractingCharacter(ai))
            yield return null;
        Complete();
    }

    protected override bool ExecutableCheck()
    {
        return true;
    }
}
