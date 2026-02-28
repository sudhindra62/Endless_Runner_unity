using UnityEngine;

public class QuestObjective : MonoBehaviour
{
    public string description;

    public virtual bool IsComplete()
    {
        return false;
    }
}
