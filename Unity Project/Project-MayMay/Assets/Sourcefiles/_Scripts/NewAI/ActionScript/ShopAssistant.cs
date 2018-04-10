using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopAssistant", menuName = "Jobs/ShopAssistant", order = 1)]
public class ShopAssistant : RootActionMulFrameable
{
    private ShopKeeping boss;
    private ShopKeeping Boss
    {
        get
        {
            if (boss == null)
                boss = Stat<QuestAssistant>().LeaderQuest.GetAction<ShopKeeping>();
            return boss;
        }
    }

    private Shop Shop
    {
        get
        {
            return Boss.Shop;
        }
    }

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override int GetReturnValue()
    {
        return Min + 1;
    }

    private List<Shop.ItemStack> stacks;
    public override IEnumerator LifeTime()
    {
        stacks = Shop.GetRestockable();
        if(stacks.Count > 0)
        {
            //restock
            Debug.Log("Restock");
        }

        yield return new WaitForSeconds(ai.senses.settings.frequency);
        Complete();
    }

    protected override bool ExecutableCheck()
    {
        return Boss.Open;
    }

    public override Transform PosTrans()
    {
        return Boss.ai.transform;
    }
}
