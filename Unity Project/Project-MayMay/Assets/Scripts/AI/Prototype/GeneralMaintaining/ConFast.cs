using System.Collections;
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
        Debug.Log("Socialize");
        
        stationaryTime = 0;
        social.SetConversationPartner();
        
        float remaining = duration;
        Vector3 lastPos = social.conversationPartner.Pos;

        while (remaining > 0) //TIJDENS deze loop vernakt het, het is niet de newevent, het lopen
        {
            remaining -= Time.deltaTime;
            if (lastPos == social.conversationPartner.Pos)
                stationaryTime += Time.deltaTime;
            else
            {
                lastPos = social.conversationPartner.Pos;
                stationaryTime = 0;
            }

            ai.Move(lastPos);
            yield return null;
        }

        Complete();
    }

    public override void Complete()
    {
        Debug.Log(social.conversationPartner);
        RewardOther();
        social.conversationPartner.Social.conversationPartner = null;
        social.conversationPartner = null;
        base.Complete();
    }

    public override int GetReturnValue()
    {
        return Uninportant;
    }

    public override void Cancel()
    {
        stationaryTime = 0;
        social.conversationPartner = null;
        base.Cancel();
    }

    private void RewardOther()
    {
        social.conversationPartner.Social.AddValue(Uninportant);
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
