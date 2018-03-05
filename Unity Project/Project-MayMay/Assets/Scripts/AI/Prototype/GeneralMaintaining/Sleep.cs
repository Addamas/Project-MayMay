using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sleep", menuName = "Actions/Sleep", order = 1)]
<<<<<<< HEAD
public class Sleep : SimpleRootAction
{
    public override void Cancel()
    {
        ai.StopCoroutine(execute);
    }

    public override void Execute()
    {
        execute = ai.StartCoroutine(_Execute());
    }

    private float start;
    private Coroutine execute;
    private IEnumerator _Execute()
    {
        start = Gamemanager.time;
        yield return new WaitForSeconds(STAT<Quest>().GetDuration());
        Complete();
    }

    public override int GetReturnValue()
    {
        return 1;
    }

=======
public class Sleep : SimpleQuest
{
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d
    public override Vector3 Pos()
    {
        return GetInteractableX.SPGetAllX<Bed>(ai)[0].transform.position;
    }
}
