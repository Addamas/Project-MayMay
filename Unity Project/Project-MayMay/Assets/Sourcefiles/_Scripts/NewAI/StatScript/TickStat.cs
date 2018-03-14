using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tickstat", menuName = "Stats/Generic/TickStat", order = 1)]
public class TickStat : NormalStat
{
    [SerializeField]
    protected float tickFrequency;

    public override void Init(GHOPE ai)
    {
        base.Init(ai);
        ai.StartCoroutine(Tick());
    }

    protected virtual IEnumerator Tick()
    {
        while (true)
        {
            yield return new WaitForSeconds(tickFrequency);
            Value--;
        }
    }
}
