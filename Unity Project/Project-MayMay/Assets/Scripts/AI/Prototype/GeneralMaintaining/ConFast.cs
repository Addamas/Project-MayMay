﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jext;

[CreateAssetMenu(fileName = "ConFast", menuName = "Actions/ConFast", order = 1)]
public class ConFast : Converse
{
    [SerializeField]
    private float interactDisModifierWhenResting, idleTimeUntilRest;
    private float stationaryTime;

    protected override IEnumerator _Execute()
    {
        stationaryTime = 0;
        conversationPartner = Social.GetSocialPartner();

        float remaining = duration;
        Vector3 lastPos = conversationPartner.Pos;
        while (remaining > 0)
        {
            remaining -= Time.deltaTime;
            if (lastPos == conversationPartner.Pos)
                stationaryTime += Time.deltaTime;
            else
                stationaryTime = 0;
            yield return null;
        }
        Complete();
    }

    public override Vector3 Pos()
    {
        if(execute != null)
            if(stationaryTime > idleTimeUntilRest)
                try
                {
                    return GetRestPosition();
                }
                catch
                {
                    return base.Pos();
                }
        return base.Pos();
    }

    private Vector3 GetRestPosition()
    {
        List<RestPosition> ret = Gamemanager.GetAllTInScene<RestPosition>();
        ret.RemoveAll(x => Vector3.Distance(ai.transform.position, x.transform.position) > interactDisModifierWhenResting);
        ret.SortByClosest(ai.transform.position);
        return ret[0].transform.position;
    }
}
