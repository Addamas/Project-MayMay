using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OpenShop", menuName = "Actions/OpenShop", order = 1)]
public class OpenShop : ShopkeepAction {

    public override void Complete()
    {
        shop.open = true;
        base.Complete();
    }
}
