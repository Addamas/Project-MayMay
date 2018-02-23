using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class Character : Jai {

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public NavMeshAgent agent;
    public float interactDistance = 1;

    public List<Interactable> ownedInteractables = new List<Interactable>();
    public List<Item> ownedItems = new List<Item>();

    public List<Other> associates = new List<Other>();
    [HideInInspector]
    public List<Character> restSocials = new List<Character>();
    public Conversation defaultConversation;

    [Serializable]
    public class Other
    {
        public Character character;
        public List<Conversation> conversations = new List<Conversation>(); //someone can say multiple things before switching to the other person

        //quest/action stuff
        public bool dinnerable;

        public Vector3 Pos
        {
            get
            {
                return character.transform.position;
            }
        }

        public Social Social
        {
            get
            {
                return character.Social;
            }
        }
    }

    [Serializable]
    public class Conversation : IComparable<Conversation>
    {
        public ConversationPart[] data;

        public int CompareTo(Conversation other)
        {
            return other.Duration - Duration;
        }

        public int Duration
        {
            get
            {
                int ret = 0;
                foreach (ConversationPart cP in data)
                    foreach (string s in cP.data)
                        ret += s.Length;
                return ret;
            }
        }
    }

    [Serializable]
    public class ConversationPart
    {
        public string[] data;
    }

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
        Gamemanager.socialables.ForEach(x => restSocials.Add(x.ai));
        foreach (Other other in associates)
            restSocials.Remove(other.character);
        restSocials.Remove(this);

        base.LateActivate();
    }

    protected virtual void SetupReferences()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override void ExecuteNext(Action action)
    {
        //Debug.Log(action.name);
        if(move != null)
            StopCoroutine(move);
        if (action.IsInRange())
            base.ExecuteNext(action);
        else
            move = StartCoroutine(Move(action));
    }

    private Coroutine move; //make this a variable return seconds for optimization
    protected virtual IEnumerator Move(Action action)
    {
        while(action.Executable)
        {
            if (action.IsInRange())
                break;
            agent.SetDestination(action.Pos());
            yield return null;
        }

        base.ExecuteNext(action);
    }

    public void Move(Vector3 pos)
    {
        agent.SetDestination(pos);
    }
}
