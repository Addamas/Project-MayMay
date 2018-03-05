using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Jext;

[CreateAssetMenu(fileName = "Social", menuName = "Stats/Social", order = 1)]
public class Social : Hunger {

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
        }
    }

    public override void Init(Jai ai)
    {
        base.Init(ai);
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
    }

    public override void AddValue(int val)
    {
        base.AddValue(val);
        val = Mathf.Clamp(val, ai.criticalLevel, 100);
    }

    private bool InRangeSocialable(Social other)
    {
        return Vector3.Distance(other.ai.transform.position, ai.transform.position) < interactDistance;
    }
}
