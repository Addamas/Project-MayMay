using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Yawn", menuName = "Actions/Yawn", order = 1)]
public class Yawn : SimpleRootAction
{
    [SerializeField]
    private float duration;

    public override void Cancel()
    {
        if(yawn != null)
            ai.StopCoroutine(yawn);
    }

    public override void Execute()
    {
        yawn = ai.StartCoroutine(Yawning());
    }

    private Coroutine yawn;
    private IEnumerator Yawning()
    {
        yield return new WaitForSeconds(duration);
        Complete();
    }

    public override float GetEstimatedTimeRequired()
    {
        return duration;
    }

    public override int GetReturnValue()
    {
        return 0;
    }

    public override bool IsInRange()
    {
        return true;
    }

    public override Vector3 Pos()
    {
        throw new System.NotImplementedException();
    }
}
