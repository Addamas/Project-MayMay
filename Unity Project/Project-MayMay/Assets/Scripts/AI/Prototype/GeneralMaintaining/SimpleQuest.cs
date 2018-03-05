using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleQuest : SimpleRootAction {

    public override void Cancel()
    {
        ai.StopCoroutine(execute);
    }

    public override void Execute()
    {
        execute = ai.StartCoroutine(_Execute());
    }

    protected float start;
    protected Coroutine execute;
    protected IEnumerator _Execute()
    {
        start = Gamemanager.time;
        yield return new WaitForSeconds(STAT<Quest>().GetDuration());
        Complete();
    }

    public override int GetReturnValue()
    {
        return 1;
    }

    public override Vector3 Pos()
    {
        throw new System.NotImplementedException();
    }
}
