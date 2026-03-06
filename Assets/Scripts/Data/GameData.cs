
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public float[] playerPosition;
    public int playerHealth;
    public List<ItemData> inventory;
    public List<QuestData> quests;
    public PlayerAchievementData achievementData;
}
