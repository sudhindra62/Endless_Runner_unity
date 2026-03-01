
using System;
using UnityEngine;

public class XPManager : Singleton<XPManager>
{
    public static event Action<int> OnLevelUp;
    public static event Action<int, int> OnXPChanged;

    [SerializeField] private ProgressionData progressionData;
    
    public int CurrentLevel { get; private set; } = 1;
    public int CurrentXP { get; private set; } = 0;

    private const string CurrentLevelKey = "PlayerLevel";
    private const string CurrentXPKey = "PlayerXP";
    private const string LastRunXpKey = "LastRunXp";

    protected override void Awake()
    {
        base.Awake();
        LoadXPData();
    }

    private void LoadXPData()
    {
        CurrentLevel = PlayerPrefs.GetInt(CurrentLevelKey, 1);
        CurrentXP = PlayerPrefs.GetInt(CurrentXPKey, 0);
        PlayerPrefs.DeleteKey(LastRunXpKey);
    }

    private void SaveXPData()
    {
        PlayerPrefs.SetInt(CurrentLevelKey, CurrentLevel);
        PlayerPrefs.SetInt(CurrentXPKey, CurrentXP);
        PlayerPrefs.Save();
    }

    public void AddXP(int amount, string source)
    {
        if (amount <= 0) return;

        if (source == "RunComplete")
        {
            if (PlayerPrefs.HasKey(LastRunXpKey))
            {
                Debug.LogWarning("Duplicate XP addition from run detected. Ignoring.");
                return;
            }
            PlayerPrefs.SetInt(LastRunXpKey, amount);
        }

        CurrentXP += amount;
        CheckForLevelUp();
        OnXPChanged?.Invoke(CurrentXP, GetXPForNextLevel());
        SaveXPData();
        Debug.Log($"Added {amount} XP from {source}. New XP: {CurrentXP}");
    }

    private void CheckForLevelUp()
    {
        int xpForNextLevel = GetXPForNextLevel();
        while (CurrentXP >= xpForNextLevel && xpForNextLevel > 0)
        {
            CurrentLevel++;
            CurrentXP -= xpForNextLevel;
            
            // Grant Skill Point
            if (SkillTreeManager.Instance != null)
            {
                SkillTreeManager.Instance.AddSkillPoints(1);
            }

            OnLevelUp?.Invoke(CurrentLevel);
            RewardManager.Instance.GrantLevelUpReward(CurrentLevel);
            Debug.Log($"Leveled up to level {CurrentLevel}! Gained 1 Skill Point.");
            
            xpForNextLevel = GetXPForNextLevel();
        }
    }

    public int GetXPForNextLevel()
    {
        return progressionData.GetXPForLevel(CurrentLevel + 1);
    }

    [ContextMenu("Reset XP and Level")]
    public void ResetProgression()
    {
        PlayerPrefs.DeleteKey(CurrentLevelKey);
        PlayerPrefs.DeleteKey(CurrentXPKey);
        PlayerPrefs.DeleteKey(LastRunXpKey);
        CurrentLevel = 1;
        CurrentXP = 0;
        OnXPChanged?.Invoke(CurrentXP, GetXPForNextLevel());
        Debug.Log("Player progression has been reset.");
    }
}
