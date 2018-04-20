using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Jext;

[CreateAssetMenu(fileName = "Social", menuName = "Stats/Social", order = 1)]
public class Social : TickStat {

    public List<Conversation> genericConversations = new List<Conversation>();

    public float timePerChar;

    [Range(0, 100)]
    public int minValue;

    public enum ConversationType {Normal, Buying, Searching }

    [Serializable]
    public class Conversation
    {
        [Tooltip("Optional")]
        public ConversationType tag;
        public List<ConPart> parts = new List<ConPart>();
    }

    [Serializable]
    public class ConPart
    {
        public List<string> parts = new List<string>();
    }

    public Conversation GetConversation(ConversationType tag)
    {
        List<Conversation> conversations = new List<Conversation>();
        foreach (Conversation conversation in genericConversations)
            if (conversation.tag == tag)
                conversations.Add(conversation);

        return conversations.RandomItem();
    }

    public Conversation GetConversation(ConversationType tag, Memory.Other other)
    {
        try
        {
            List<Conversation> conversations = ai.memory.GetInfoCharacter(other.character).conversations;
            foreach (Conversation conversation in conversations)
                if (conversation.tag == tag)
                    return conversation;
            return null;
        }
        catch
        {
            return GetConversation(tag);
        }
    }

    public override void SetValue(int value)
    {
        base.SetValue(Mathf.Clamp(value, minValue, Max));
    }

    public IEnumerator Speak(ConPart conPart)
    {
        string sentence;
        float duration;
        for (int i = 0; i < conPart.parts.Count; i++)
        {
            sentence = conPart.parts[i];
            Debug.Log(ai.name);
            
            duration = sentence.Length * timePerChar;
            DialogueManager.instance.ConvertTextVisually(ai, sentence, duration);

            yield return new WaitForSeconds(duration);
        }
    }
}