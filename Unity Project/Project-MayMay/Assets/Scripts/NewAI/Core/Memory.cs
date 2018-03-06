using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Memory : CharacterExtension
{
    [SerializeField]
    private List<Other> relatives = new List<Other>();
    [SerializeField, Range(0,100)]
    private int defaultAffinity;
    [SerializeField]
    private int memorySlots;

    //own past agenda

    [Serializable]
	public class Other : IComparable<Other>
    {
        public Character character;
        [Range(0, 100)]
        public int affinity;
        [NonSerialized]
        public int lastSpotted;

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
            if (action.special)
                specialMemories.Add(new MemorySlot(action, TimeManager.time));
            lastSpotted = time;
        }
    }

    public void AddMemory(Other other, Action action)
    {
        other.AddMemory(action, memorySlots, TimeManager.time);
    }

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

        public int CompareTo(MemorySlot other)
        {
            return other.time - time;
        }
    }

    public void Init()
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