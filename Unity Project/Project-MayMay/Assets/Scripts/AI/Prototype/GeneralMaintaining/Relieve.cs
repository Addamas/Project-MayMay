using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Relieve", menuName = "Actions/Relieve", order = 1)]
public class Relieve : SimpleRootAction
{
    private float duration;

    public override void Cancel()
    {
        if (execute != null)
            ai.StopCoroutine(execute);
    }

    public override void Execute()
    {
        execute = ai.StartCoroutine(_Execute());
    }

    private Coroutine execute;
    private IEnumerator _Execute()
    {
        yield return new WaitForSeconds(duration);
        Complete();
    }

    public override float GetEstimatedTimeRequired()
    {
        return duration + base.GetEstimatedTimeRequired();
    }

    public override int GetReturnValue()
    {
        return Uninportant;
    }

    public override Vector3 Pos()
    {
        return GetInteractableX.SPGetAllX<WC>(ai)[0].transform.position;
    }
}
