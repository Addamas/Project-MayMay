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
        List<Link> ret = new List<Link>();
        if (GetStorageInteractable() != null)
            ret.Add(Link.Interacted);
        return ret;
    }

    private StackInteractable GetStackInteractable()
    {
        
        return null;
    }

    private StackInteractable GetStorageInteractable()
    {
        List<StackInteractable> emptyStacks = Shop.GetRestockable();
        Shop shop = Shop;
        foreach (StackInteractable stack in emptyStacks)
            foreach (Shop.ItemStack storageStack in shop.storage)
                if (stack.type == storageStack.Type)
                    foreach (StackInteractable interactable in storageStack.stack)
                        if (interactable.Filled)
                            return interactable;
        return null;
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
        return true;
    }

    public override Transform PosTrans()
    {
        return Boss.ai.transform;
    }

    public override void Prepare()
    {
        StackInteractable interactable = GetStackInteractable();

        if (interactable == null)
            interactable = GetStorageInteractable();
        if(interactable != null)
            ai.GetAction<Interact>().target = interactable;

        base.Prepare();
    }
}
