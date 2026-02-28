using UnityEngine;
using System;

/// <summary>
/// Manages player level and experience points.
/// </summary>
public class PlayerProgressionManager : MonoBehaviour
{
    public event Action<int> OnLevelUp;
    public event Action<int, int> OnXPChanged;

    public int currentLevel { get; private set; } = 1;
    public int currentXP { get; private set; } = 0;

    public ProgressionData progressionData;

    private void Awake()
    {
        ServiceLocator.Register<PlayerProgressionManager>(this);
        // Load player progression here
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<PlayerProgressionManager>();
    }

    public void AddXP(int amount)
    {
        if (amount <= 0) return;
        currentXP += amount;
        CheckForLevelUp();
        OnXPChanged?.Invoke(currentXP, GetXPForNextLevel());
    }

    private void CheckForLevelUp()
    {
        int xpForNextLevel = GetXPForNextLevel();
        while (currentXP >= xpForNextLevel)
        {
            currentLevel++;
            currentXP -= xpForNextLevel;
            OnLevelUp?.Invoke(currentLevel);
            xpForNextLevel = GetXPForNextLevel();
        }
    }

    public int GetXPForNextLevel()
    {
        return progressionData.GetXPForLevel(currentLevel + 1);
    }
}
