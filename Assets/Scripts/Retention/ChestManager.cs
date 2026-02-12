using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the state and timers for all reward chests.
/// This is a persistent singleton that saves cooldown data to PlayerPrefs.
/// </summary>
public partial class ChestManager : MonoBehaviour
{
    public static ChestManager Instance { get; private set; }

    [Header("Configuration")]
    [Tooltip("A list of all ChestData ScriptableObjects, one for each chest type.")]
    [SerializeField] private List<ChestData> allChests = new List<ChestData>();

    // Live state of all chests, keyed by ChestType.
    private Dictionary<ChestType, ChestState> chestStates = new Dictionary<ChestType, ChestState>();

    // Event triggered when a chest's state changes.
    public static event Action<ChestType> OnChestStateChanged;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeChestStates();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Attempts to claim the rewards for a specific chest.
    /// </summary>
    public void ClaimChest(ChestType chestType)
    {
        if (!chestStates.ContainsKey(chestType) || !IsChestReady(chestType))
        {
            Debug.LogWarning($"Cannot claim chest {chestType}. Not ready or does not exist.");
            return;
        }

        ChestData chestData = GetChestData(chestType);
        if (chestData == null) return;

        RewardPackage rewards = chestData.GenerateRewards();
        RewardManager.Instance.GrantRewards(rewards);

        chestStates[chestType].isReady = false;
        chestStates[chestType].nextAvailableTime =
            DateTime.UtcNow.AddHours(chestData.cooldownHours);

        SaveChestState(chestType);
        OnChestStateChanged?.Invoke(chestType);
    }

    public ChestData GetChestData(ChestType chestType) =>
        allChests.FirstOrDefault(c => c.type == chestType);

    public bool IsChestReady(ChestType chestType) =>
        chestStates.ContainsKey(chestType) && chestStates[chestType].isReady;

    public DateTime GetNextAvailableTime(ChestType chestType) =>
        chestStates[chestType].nextAvailableTime;

    private void InitializeChestStates()
    {
        foreach (var chestData in allChests)
        {
            chestStates[chestData.type] = LoadChestState(chestData.type);
        }
    }

    void Update()
    {
        foreach (var chestData in allChests)
        {
            var state = chestStates[chestData.type];
            if (!state.isReady && DateTime.UtcNow >= state.nextAvailableTime)
            {
                state.isReady = true;
                OnChestStateChanged?.Invoke(chestData.type);
            }
        }
    }

    #region Persistence

    [Serializable]
    private class ChestState
    {
        public bool isReady = true;
        public string nextAvailableTimeStr;

        public DateTime nextAvailableTime
        {
            get => DateTime.TryParse(nextAvailableTimeStr, out var dt) ? dt : DateTime.UtcNow;
            set => nextAvailableTimeStr = value.ToString("o");
        }
    }

    private void SaveChestState(ChestType chestType)
    {
        PlayerPrefs.SetString(
            $"ChestState_{chestType}_V1",
            JsonUtility.ToJson(chestStates[chestType])
        );
        PlayerPrefs.Save();
    }

    private ChestState LoadChestState(ChestType chestType)
    {
        string key = $"ChestState_{chestType}_V1";
        return PlayerPrefs.HasKey(key)
            ? JsonUtility.FromJson<ChestState>(PlayerPrefs.GetString(key))
            : new ChestState { isReady = true, nextAvailableTime = DateTime.UtcNow };
    }

    #endregion
}
