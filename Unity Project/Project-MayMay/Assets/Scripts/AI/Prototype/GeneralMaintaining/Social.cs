using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Jext;

[CreateAssetMenu(fileName = "Social", menuName = "Stats/Social", order = 1)]
public class Social : Hunger {

    [SerializeField]
    private float interactDistance;
    [NonSerialized]
    public Character.Other conversationPartner = null;
    private Character.Other defaultConversationPartner = new Character.Other();
    public bool Conversing
    {
        get
        {
            return conversationPartner != null;
        }
    }

    public Vector3 Pos
    {
        get
        {
            return ai.transform.position;
        }
    }

    public override void Init(Jai ai)
    {
        base.Init(ai);
        defaultConversationPartner.conversations.Add(this.ai.defaultConversation);
    }

    public Character.Other GetAssociate()
    {
        List<Character.Other> socialPartners = new List<Character.Other>();

        ai.associates.ForEach(x => socialPartners.Add(x));
        socialPartners.RemoveAll(x => !InRangeSocialable(x.character.Social));

        return socialPartners.RandomItem();
    }

    public Social GetRandom()
    {
        List<Social> restSocialPartners = new List<Social>();

        foreach (Character other in ai.restSocials)
            if (InRangeSocialable(other.Social))
                restSocialPartners.Add(other.Social);

        return restSocialPartners.RandomItem();
    }

    public void SetConversationPartner()
    {
        try
        {
            conversationPartner = GetAssociate();
        }
        catch
        {
            SetRandom(GetRandom());
        }
        finally
        {
            conversationPartner.Social.SetConversationPartner(ai);
        }
    }

    public void SetConversationPartner(Character social)
    {
        foreach (Character.Other other in ai.associates)
            if(other.character == social)
            {
                conversationPartner = other;
                return;
            }
        SetRandom(social.Social);
    }

    private void SetRandom(Social social)
    {
        conversationPartner = defaultConversationPartner;
        conversationPartner.character = social.ai;
    }

    public override void AddValue(int val)
    {
        base.AddValue(val);
        value = Mathf.Clamp(value, ai.criticalLevel, Uninportant);
    }

    public void AddValueFromCharacter(Social social)
    {
        AddValue(Uninportant);
    }

    private bool InRangeSocialable(Social other)
    {
        return Vector3.Distance(other.Pos, ai.transform.position) < interactDistance;
    }
}
