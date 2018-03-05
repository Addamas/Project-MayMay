using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Jext;

[CreateAssetMenu(fileName = "Social", menuName = "Stats/Social", order = 1)]
public class Social : Hunger {

<<<<<<< HEAD
    public int defaultAffinity;
    [SerializeField]
    private float interactDistance;

    [Serializable]
    public class Other
    {
        public Character character;
        public int affinity;
        [HideInInspector]
        public Social social;

        public Vector3 Pos
        {
            get
            {
                return character.transform.position;
            }
=======
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
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d
        }
    }

    public override void Init(Jai ai)
    {
        base.Init(ai);
<<<<<<< HEAD
    }

    public Other GetSocialPartner()
    {
        List<Other> socialPartners = new List<Other>();
        socialPartners.AddList(ai.associates, false);
        socialPartners.RemoveAll(x => !InRangeSocialable(x.social));
        if(socialPartners.Count > 0)
            return socialPartners.RandomItem();

        List<Social> restSocialPartners = new List<Social>();
        foreach (Social social in ai.restSocials)
            if (InRangeSocialable(social))
                restSocialPartners.Add(social);
        Social ret = restSocialPartners.RandomItem();
        return new Other { character = ret.ai, affinity = defaultAffinity, social = ret };
=======
        defaultConversationPartner.conversations.Add(this.ai.defaultConversation);
    }

    public Character.Other GetAssociate()
    {
        return GetAssociates().RandomItem();
    }

    public List<Character.Other> GetAssociates()
    {
        List<Character.Other> socialPartners = new List<Character.Other>();

        ai.associates.ForEach(x => socialPartners.Add(x));
        socialPartners.RemoveAll(x => !InRangeSocialable(x.character.Social));
        return socialPartners;
    }

    public Social GetRandom()
    {
        return GetRandoms().RandomItem();
    }

    public List<Social> GetRandoms()
    {
        List<Social> restSocialPartners = new List<Social>();

        foreach (Character other in ai.restSocials)
            if (InRangeSocialable(other.Social))
                restSocialPartners.Add(other.Social);

        return restSocialPartners;
    }

    public List<Social> GetAll()
    {
        List<Character.Other> associates = GetAssociates();
        List<Social> all = GetRandoms();
        associates.ForEach(x => all.Add(x.Social));
        return all;
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
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d
    }

    public override void AddValue(int val)
    {
        base.AddValue(val);
<<<<<<< HEAD
        val = Mathf.Clamp(val, ai.criticalLevel, 100);
=======
        value = Mathf.Clamp(value, ai.criticalLevel, Uninportant);
    }

    public void AddValueFromCharacter(Social social)
    {
        AddValue(Uninportant);
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d
    }

    private bool InRangeSocialable(Social other)
    {
<<<<<<< HEAD
        return Vector3.Distance(other.ai.transform.position, ai.transform.position) < interactDistance;
=======
        return Vector3.Distance(other.Pos, ai.transform.position) < interactDistance;
>>>>>>> f940348b70633fa0f0d03e0b7299d6ceaf7f1e5d
    }
}
