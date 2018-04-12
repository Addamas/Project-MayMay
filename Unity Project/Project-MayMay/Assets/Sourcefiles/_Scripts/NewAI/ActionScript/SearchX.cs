using Jext;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SearchX", menuName = "Actions/SearchX", order = 1)]
public class SearchX : ConverseNormal {

    [NonSerialized]
    public Character target;
    private Area targetArea;

    [SerializeField]
    private float areaSeachSize;

    private List<Character> spokenCharacters = new List<Character>();

    public override List<Link> GetReturnValue()
    {
        return new List<Link>() {Link.HasPerson };
    }

    public bool Searching
    {
        get
        {
            return target != null;
        }
    }

    protected override bool ExecutableCheck()
    {
        if (InRange())
            return base.ExecutableCheck();
        return target != null;
    }

    protected override bool AvailableCheck(Memory.Other other)
    {
        if (spokenCharacters.Contains(other.character))
            return false;
        return base.AvailableCheck(other);
    }

    public override Transform PosTrans()
    {
        if (targetArea == null)
            return base.PosTrans();
        return Vector3.Distance(targetArea.transform.position, ai.transform.position) > areaSeachSize ? targetArea.transform : base.PosTrans();
    }

    protected override Social.Conversation PickConversation(Memory.Other other)
    {
        return GetConversation(other, Social.ConversationType.Searching);
    }

    protected override IEnumerator WhileLinked(Memory.Other other)
    {
        spokenCharacters.Add(other.character);

        List<Memory.MemorySlot> memories = new List<Memory.MemorySlot>();

        foreach (Memory.MemorySlot memorySlot in other.Memories)
            if (memorySlot.action.ai == other.character)
                memories.Add(memorySlot);

        memories.Sort();

        if (memories.Count > 0)
        {
            Memory.MemorySlot slot = memories.First();
            target = slot.action.ai;
            targetArea = slot.area;
        }

        return base.WhileLinked(other);
    }

    protected override void OnFinished()
    {
        spokenCharacters.Clear();
        target = null;
        base.OnFinished();
    }
}