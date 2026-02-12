using UnityEngine;
using System;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    [Header("Leveling Curve")]
    [SerializeField] private int baseXPRequirement = 100;
    [SerializeField] private float levelExponent = 1.1f;

    [Header("XP Sources")]
    [SerializeField] private int xpPerSecond = 10;
    [SerializeField] private int xpPerCoin = 1;

    private const string ProgressionDataKey = "PlayerProgression_Data";

    private PlayerProgressionData progressionData;

    public static event Action<PlayerProgressionData> OnXPChanged;
    public static event Action<PlayerProgressionData> OnLeveledUp;

    private float runTimer = 0f;
    private bool isRunActive = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgression();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isRunActive)
        {
            runTimer += Time.deltaTime;
            if (runTimer >= 1f)
            {
                int xpFromTime = Mathf.FloorToInt(runTimer * xpPerSecond);
                AddXP(xpFromTime, false);
                runTimer = 0f;
            }
        }
    }

    public void AddXP(int amount, bool save = true)
    {
        if (amount <= 0) return;

        progressionData.CurrentXP += amount;
        progressionData.TotalXP += amount;

        bool hasLeveledUp = false;
        while (progressionData.CurrentXP >= progressionData.XPToNextLevel)
        {
            LevelUp();
            hasLeveledUp = true;
        }

        OnXPChanged?.Invoke(progressionData);
        if (hasLeveledUp)
            OnLeveledUp?.Invoke(progressionData);

        if (save) SaveProgression();
    }

    public void StartRun()
    {
        isRunActive = true;
        runTimer = 0f;
    }

    public void EndRun(int coinsCollectedThisRun)
    {
        // 🔹 FIXED TYPO — REQUIRED FOR COMPILATION
        isRunActive = false;

        AddXP(coinsCollectedThisRun * xpPerCoin, true);
        SaveProgression();
    }

    public PlayerProgressionData GetProgressionData() => progressionData;
    public float GetProgress01() => (float)progressionData.CurrentXP / progressionData.XPToNextLevel;

    private void LevelUp()
    {
        progressionData.CurrentXP -= progressionData.XPToNextLevel;
        progressionData.CurrentLevel++;
        progressionData.XPToNextLevel = CalculateXPForNextLevel();
        Debug.Log($"Leveled Up! New Level: {progressionData.CurrentLevel}");
    }

    private int CalculateXPForNextLevel()
    {
        return baseXPRequirement +
               (int)(baseXPRequirement * Mathf.Pow(progressionData.CurrentLevel, levelExponent));
    }

    private void SaveProgression()
    {
        string json = JsonUtility.ToJson(progressionData);
        PlayerPrefs.SetString(ProgressionDataKey, json);
        PlayerPrefs.Save();
    }

    private void LoadProgression()
    {
        string json = PlayerPrefs.GetString(ProgressionDataKey, "");
        if (!string.IsNullOrEmpty(json))
        {
            progressionData = JsonUtility.FromJson<PlayerProgressionData>(json);
        }
        else
        {
            progressionData = new PlayerProgressionData
            {
                CurrentLevel = 1,
                CurrentXP = 0,
                TotalXP = 0,
                XPToNextLevel = CalculateXPForNextLevel()
            };
        }
        OnXPChanged?.Invoke(progressionData);
    }
}
