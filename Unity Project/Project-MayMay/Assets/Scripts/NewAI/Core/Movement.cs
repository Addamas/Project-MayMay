using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Movement : CharacterExtension
{
    [NonSerialized]
    public NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
    }
    
    public void Follow(Transform trans)
    {
        Resume();
        follow = StartCoroutine(_Follow(trans));
    }

    private Coroutine follow;
    private IEnumerator _Follow(Transform trans)
    {
        while (true)
        {
            agent.SetDestination(trans.position);
            yield return null;
        }
    }

    private void Resume()
    {
        Unfollow();
        agent.isStopped = false;
    }

    public void Stop()
    {
        Unfollow();
        agent.isStopped = true;
    }

    private void Unfollow()
    {
        if (follow != null)
            StopCoroutine(follow);
    }

    public override void Init()
    {
        
    }
}
