using Jext;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SearchX", menuName = "Actions/SearchX", order = 1)]
public class SearchX : Converse {

    [SerializeField]
    private float areaSeachSize;

    private SearchCharacterStat SearchStat
    {
        get
        {
            return Stat<SearchCharacterStat>();
        }
    }

    private Character Target
    {
        get
        {
            return SearchStat.target;
        }
        set
        {
            SearchStat.target = value;
        }
    }

    private Area TargetArea
    {
        get
        {
            return SearchStat.targetPredictedArea;
        }
        set
        {
            SearchStat.targetPredictedArea = value;
        }
    }

    private List<Character> spokenCharacters = new List<Character>();

    protected override bool ExecutableCheck()
    {
        if (InRange())
            return base.ExecutableCheck();
        return Target != null;
    }

    protected override bool AvailableCheck(Memory.Other other)
    {
        if (spokenCharacters.Contains(other.character))
            return false;
        return base.AvailableCheck(other);
    }

    public override Transform PosTrans()
    {
        if (TargetArea == null)
            return base.PosTrans();
        return Vector3.Distance(TargetArea.transform.position, ai.transform.position) > areaSeachSize ? TargetArea.transform : base.PosTrans();
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
            Target = slot.action.ai;
            TargetArea = slot.area;
        }

        return base.WhileLinked(other);
    }

    protected override void OnFinished()
    {
        if(Target == null)
            spokenCharacters.Clear();
    }
}