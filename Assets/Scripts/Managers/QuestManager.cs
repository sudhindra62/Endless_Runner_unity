using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public void StartQuest(string questName)
    {
        Debug.Log($"Quest started: {questName}");
    }
}