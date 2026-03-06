
using System;

[Serializable]
public class PlayerAchievementProgress
{
    public AchievementID id;
    public string name;
    public string description;
    public int requiredValue;
    public int currentValue;
    public bool isCompleted;
    public string rewardId;

    public void IncrementProgress(int amount)
    {
        if (isCompleted) return;
        currentValue += amount;
        if (currentValue >= requiredValue)
        {
            isCompleted = true;
            // Trigger event or notification
        }
    }
}
