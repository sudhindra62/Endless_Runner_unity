
[System.Serializable]
public class SaveData
{
    public PlayerAchievementData achievementData;
    public int currentLevel;
    public long currentXP;
    public byte[] bestGhostRunData;

    public SaveData()
    {
        achievementData = new PlayerAchievementData();
        currentLevel = 1;
        currentXP = 0;
        bestGhostRunData = null;
    }
}
