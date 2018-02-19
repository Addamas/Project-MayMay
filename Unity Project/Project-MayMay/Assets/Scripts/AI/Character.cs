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

    public List<Social.Other> associates = new List<Social.Other>();
    [HideInInspector]
    public List<Social> restSocials = new List<Social>();

    public Social Social
    {
        get
        {
            foreach (Stat stat in stats)
                if (stat as Social != null)
                    return stat as Social;
            return null;
        }
    }

    public override void Activate()
    {
        SetupReferences();
        base.Activate();
    }

    public override void LateActivate()
    {
        associates.ForEach(x => x.social = x.character.Social);
        Gamemanager.socialables.ForEach(x => restSocials.Add(x));
        foreach (Social.Other other in associates)
            restSocials.Remove(other.social);
        restSocials.Remove(Social);
        base.LateActivate();
    }

    protected virtual void SetupReferences()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void ExecuteNext(Action action)
    {
        if(move != null)
            StopCoroutine(move);
        if (action.IsInRange())
            base.ExecuteNext(action);
        else
            move = StartCoroutine(Move(action));
    }

    private Coroutine move;
    protected virtual IEnumerator Move(Action action)
    {
        while(action.Executable)
        {
            if (action.IsInRange())
                break;
            agent.SetDestination(action.Pos());
            yield return null;
        }
        if (action.Executable)
            base.ExecuteNext(action);
        else
            curAction = null;
    }

    public void Move(Vector3 pos)
    {
        agent.SetDestination(pos);
    }
}
