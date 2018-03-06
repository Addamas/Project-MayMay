using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Jext;

public class Memory : CharacterExtension
{
    [SerializeField]
    private List<Other> relatives = new List<Other>();
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

        public Other(Character character, int affinity)
        {
            this.character = character;
            this.affinity = affinity;
        }

        public int CompareTo(Other other)
        {
            return other.lastSpotted - lastSpotted;
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

        public void AddMemory(Action action, int limit, int time)
        {
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
                        specialMemories.Add(new MemorySlot(action, TimeManager.time));
                        for (int i = 0; i < specialMemories.Count - limit; i++)
                            specialMemories.Remove(specialMemories.Last());
                    }
                    else
                    {
                        memories.Add(new MemorySlot(action, TimeManager.time));
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

    public class MemorySlot
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
    }

    public override void Init()
    {
        List<Other> addable = new List<Other>();
        bool fit;
        foreach (Character character in GameManager.characters)
        {
            if (character == GetComponent<Character>())
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
    }

    public Other GetInfoCharacter(Character character)
    {
        foreach (Other other in relatives)
            if (other.character == character)
                return other;
        return null;
    }
}