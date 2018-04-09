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

    public override List<Link> GetRemainingLinks()
    {
        return new List<Link>();
    }

    public override int GetReturnValue()
    {
        return Min + 1;
    }

    public override IEnumerator LifeTime()
    {
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
