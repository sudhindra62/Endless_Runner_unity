
[System.Serializable]
public class SaveData
{
    public PlayerAchievementData achievementData;
    public int currentLevel;
    public long currentXP;

    public SaveData()
    {
        achievementData = new PlayerAchievementData();
        currentLevel = 1;
        currentXP = 0;
    }
}
