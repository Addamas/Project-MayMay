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
    
    public void MoveTo(Vector3 vec)
    {
        agent.isStopped = false;
        agent.SetDestination(vec);
    }

    public void Stop()
    {
        agent.isStopped = true;
    }

    public override void Init()
    {
        
    }
}
