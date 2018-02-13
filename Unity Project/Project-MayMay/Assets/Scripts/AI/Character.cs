using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Character : Jai {

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public NavMeshAgent agent;
    public float interactDistance = 1;

    public List<Interactable> ownedInteractables = new List<Interactable>();
    public List<Item> ownedItems = new List<Item>();

    public override void Activate()
    {
        SetupReferences();
        base.Activate();
    }

    protected virtual void SetupReferences()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void ExecuteNext(Action action)
    {
        if (action.IsInRange())
            base.ExecuteNext(action);
        else
            StartCoroutine(Move(action));
    }

    protected virtual IEnumerator Move(Action action)
    {
        while (!action.IsInRange())
        {
            agent.SetDestination(action.Pos());
            yield return null;
        }
        base.ExecuteNext(action);
    }
}
