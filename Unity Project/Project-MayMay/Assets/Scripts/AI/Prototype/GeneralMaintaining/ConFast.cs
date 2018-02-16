using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConFast", menuName = "Actions/ConFast", order = 1)]
public class ConFast : Converse
{
    protected override IEnumerator _Execute()
    {
        conversationPartner = Social.GetSocialPartner();
        yield return new WaitForSeconds(duration);
        Complete();
    }
}
