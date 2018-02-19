using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat_Decreasing : Stat_Normal, ITickable {

    [SerializeField]
    protected int tickSpeed;

    public virtual IEnumerator Tick()
    {
        while (value > 0)
        {
            yield return new WaitForSeconds(tickSpeed);
            AddValue(-1);
            TickInterval();
        }
    }

    protected virtual void TickInterval()
    {

    }

    public override float TimeLeftUntilEmpty()
    {
        return value / tickSpeed;
    }
}
