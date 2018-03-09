using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Social", menuName = "Stats/Social", order = 1)]
public class Social : TickStat {

    public List<Conversation> genericConversations = new List<Conversation>();

    public float rewardPerAffinity, decreasePerAffinity,
        timePerChar;

    [Range(0, 100)]
    public int minValue, breakValue;

    [Serializable]
    public class Conversation
    {
        public List<ConPart> parts = new List<ConPart>();
    }

    [Serializable]
    public class ConPart
    {
        public List<string> parts = new List<string>();
    }

    public override void SetValue(int value)
    {
        base.SetValue(Mathf.Clamp(value, minValue, Max));
    }

    public IEnumerator Speak(ConPart conPart)
    {
        string sentence;
        for (int i = 0; i < conPart.parts.Count; i++)
        {
            sentence = conPart.parts[i];
            Debug.Log(ai.name + ": " + sentence);
            yield return new WaitForSeconds(sentence.Length * timePerChar);
        }
    }
}
