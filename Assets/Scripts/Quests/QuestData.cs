
using UnityEngine;

public enum QuestType
{
    Daily,
    Weekly,
    Event
}

public enum QuestDifficulty
{
    Easy,
    Medium,
    Hard
}

[CreateAssetMenu(fileName = "QuestData", menuName = "Quests/Quest Data")]
public class QuestData : ScriptableObject
{
    public string questName;
    public QuestType questType;
    public QuestDifficulty difficulty;
    [TextArea] public string description;
    public int requiredProgress;
    public int rewardCoins;
    public int rewardXP;
    public int rewardGems;
    public GameObject rewardItemPrefab; // For chests or cosmetic fragments
    public bool isEventQuest = false;

}
