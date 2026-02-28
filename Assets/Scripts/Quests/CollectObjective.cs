using UnityEngine;

public class CollectObjective : QuestObjective
{
    public Item itemToCollect;
    public int amountToCollect;

    public override bool IsComplete()
    {
        return Inventory.Instance.items.FindAll(item => item == itemToCollect).Count >= amountToCollect;
    }
}
