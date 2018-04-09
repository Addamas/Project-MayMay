using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Jext;

public class Memory : CharacterExtension
{
    public List<Other> relatives = new List<Other>();
    [SerializeField, Range(0,100)]
    private int defaultAffinity;
    [SerializeField]
    private int memorySlots;

    //own past agenda
    private List<MemorySlot> performedActions = new List<MemorySlot>();
    public List<MemorySlot> PerformedActions
    {
        get
        {
            return performedActions;
        }
        set
        {
            performedActions = value;
            for (int i = 0; i < performedActions.Count - memorySlots; i++)
                performedActions.Remove(performedActions.Last());
        }
    }

    public class SpottedItem
    {
        public Item item;
        public Vector3 pos;

        public SpottedItem(Item item)
        {
            this.item = item;
            UpdatePos();
        }

        public void UpdatePos()
        {
            pos = item.transform.position;
        }
    }

    private List<SpottedItem> spottedItems = new List<SpottedItem>();

    public void AddSpottedItem(Item item)
    {
        foreach (SpottedItem other in spottedItems)
            if (other.item == item)
            {
                other.UpdatePos();
                return;
            }

        spottedItems.Add(new SpottedItem(item));
    }

    [Serializable]
	public class Other : IComparable<Other>
    {
        public Character character;
        [Range(0, 100)]
        public int affinity;
        [NonSerialized]
        public int lastSpotted;
        public Area lastSpottedArea;
        public List<Area> knownAreas = new List<Area>(); //where you can usually find him
        public List<Social.Conversation> conversations = new List<Social.Conversation>();

        public Other(Character character, int affinity)
        {
            this.character = character;
            this.affinity = affinity;
        }

        public int CompareTo(Other other)
        {
            return other.lastSpotted - lastSpotted;
        }

        public Social.Conversation GetConversation(Social.ConversationType type)
        {
            List<Social.Conversation> conversations = new List<Social.Conversation>();
            foreach (Social.Conversation conversation in this.conversations)
                if (conversation.tag == type)
                    conversations.Add(conversation);
            return conversations.RandomItem();
        }

        private List<MemorySlot> memories = new List<MemorySlot>(),
            specialMemories = new List<MemorySlot>();

        public List<MemorySlot> Memories
        {
            get
            {
                return memories;
            }
            set
            {
                memories = value;
                memories.Sort();
            }
        }

        public List<MemorySlot> SpecialMemories
        {
            get
            {
                return specialMemories;
            }
            set
            {
                specialMemories = value;
                specialMemories.Sort();
            }
        }

        public void ResetMemory()
        {
            memories = new List<MemorySlot>();
            specialMemories = new List<MemorySlot>();
        }

        public void AddMemory(Action action, int limit, int time)
        {
            if (action == null)
                return;

            if (action.IsExecuting())
            {
                bool fit = true;

                if (memories.Count > 0)
                    if (action == memories.Last().action)
                        fit = false;
                if (specialMemories.Count > 0)
                    if (action == specialMemories.Last().action)
                        fit = false;

                if(fit)
                    if (action.special)
                    {
                        SpecialMemories.Add(new MemorySlot(action, TimeManager.time));
                        for (int i = 0; i < specialMemories.Count - limit; i++)
                            specialMemories.Remove(specialMemories.Last());
                    }
                    else
                    {
                        Memories.Add(new MemorySlot(action, TimeManager.time));
                        for (int i = 0; i < memories.Count - limit; i++)
                            memories.Remove(memories.Last());
                    }
            }

            Area area = action.ai.GetArea();
            if (!knownAreas.Contains(area))
                knownAreas.Add(area);

            lastSpotted = time;
            lastSpottedArea = area;
        }
    }

    #region Add Memory
    public void AddMemory(Other other, Action action)
    {
        other.AddMemory(action, memorySlots, TimeManager.time);
    }

    public void AddMemory(Action action)
    {
        PerformedActions.Add(new MemorySlot(action, TimeManager.time));
    }
    #endregion

    public class MemorySlot : IComparable<MemorySlot>
    {
        public Action action;
        public Area area;
        public int time;

        public MemorySlot(Action action, int time)
        {
            this.action = action;
            area = action.ai.GetArea();
            this.time = time;
        }

        public void UpdateMemory(MemorySlot memorySlot)
        {
            if (time > memorySlot.time)
                return;

            area = memorySlot.area;
            time = memorySlot.time;
        }

        public int CompareTo(MemorySlot other)
        {
            return time - other.time;
        }
    }

    public override void Init()
    {
        List<Other> addable = new List<Other>();
        bool fit;
        foreach (Character character in GameManager.characters)
        {
            if (character == GetComponent<Character>())
                continue;
            if (!character.Socializable)
                continue;

            fit = true;
            foreach (Other other in relatives)
                if (other.character == character)
                {
                    fit = false;
                    break;
                }
            if (fit)
                addable.Add(new Other(character, defaultAffinity));
        }

        addable.ForEach(x => relatives.Add(x));
        relatives.ForEach(x => x.ResetMemory());
    }

    public Other GetInfoCharacter(Character character)
    {
        foreach (Other other in relatives)
            if (other.character == character)
                return other;
        return null;
    }
}