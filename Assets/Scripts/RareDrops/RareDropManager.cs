
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class RareDropManager : MonoBehaviour
{
    public static RareDropManager Instance { get; private set; }

    [SerializeField] private List<RareDropData> rareDropPool;

    public static event Action<RareDropType, int> OnRareDrop;

    private Dictionary<RareDropData, int> pityCounters = new Dictionary<RareDropData, int>();
    private Dictionary<RareDropData, int> dropsThisRun = new Dictionary<RareDropData, int>();

    // Anti-exploit: Cooldown to prevent rapid-fire drops from the same event
    private float lastDropTime = -Mathf.Infinity;
    private const float DROP_COOLDOWN = 1.0f; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeDrops();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        // Hook into various game events that can trigger a rare drop
        GameManager.OnCoinCollected += OnCoinCollected; // Assuming GameManager broadcasts this
        BossChaseManager.OnBossChaseEnd += OnBossSurvived;
        FlowComboManager.OnComboBroken += OnComboEnded;
        MilestoneManager.OnMilestoneReached += OnMilestoneReached;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void UnsubscribeFromEvents()
    {
        GameManager.OnCoinCollected -= OnCoinCollected;
        BossChaseManager.OnBossChaseEnd -= OnBossSurvived;
        FlowComboManager.OnComboBroken -= OnComboEnded;
        MilestoneManager.OnMilestoneReached -= OnMilestoneReached;
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void InitializeDrops()
    {
        foreach (RareDropData drop in rareDropPool)
        {
            // Load pity counter from a secure save system to prevent tampering
            pityCounters[drop] = PlayerPrefs.GetInt($"PityCounter_{drop.name}", 0); 
            dropsThisRun[drop] = 0;

            // Apply remote config overrides if available
            // drop.baseChance = RemoteConfig.GetFloat($"RareDrop_{drop.dropType}_BaseChance", drop.baseChance);
            // drop.pityThreshold = RemoteConfig.GetInt($"RareDrop_{drop.dropType}_PityThreshold", drop.pityThreshold);
            // drop.maxPerRun = RemoteConfig.GetInt($"RareDrop_{drop.dropType}_MaxPerRun", drop.maxPerRun);
            // drop.isEnabled = RemoteConfig.GetBool($"RareDrop_{drop.dropType}_IsEnabled", drop.isEnabled);
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.RunStart)
        {
            ResetRunTracking();
        }
    }

    private void ResetRunTracking()
    {
        // Reset the count of drops for the new run
        foreach (RareDropData drop in rareDropPool)
        {
            dropsThisRun[drop] = 0;
        }
    }

    private void TryTriggerDrop(int riskTier)
    {
        // Anti-exploit: Prevent multiple drops from a single event frame
        if (Time.time < lastDropTime + DROP_COOLDOWN)
        {
            return;
        }

        // 1. Filter drops by risk tier and availability
        List<RareDropData> eligibleDrops = rareDropPool
            .Where(drop => drop.isEnabled && drop.riskTier <= riskTier && dropsThisRun[drop] < drop.maxPerRun)
            .ToList();

        if (eligibleDrops.Count == 0) return;

        // 2. Calculate total weight for weighted selection
        float totalWeight = eligibleDrops.Sum(drop => drop.weight);

        // 3. Roll the dice
        float randomRoll = UnityEngine.Random.Range(0, totalWeight);
        
        // 4. Select the drop based on the weighted roll
        RareDropData selectedDrop = null;
        float cumulativeWeight = 0f;
        foreach (var drop in eligibleDrops)
        {
            cumulativeWeight += drop.weight;
            if (randomRoll < cumulativeWeight)
            {
                selectedDrop = drop;
                break;
            }
        }

        if (selectedDrop == null) return;

        // 5. Evaluate the chance of the selected drop
        float pityBonus = (pityCounters[selectedDrop] >= selectedDrop.pityThreshold) 
            ? (pityCounters[selectedDrop] - selectedDrop.pityThreshold) * 0.01f // Example: 1% bonus per count over threshold
            : 0f;
        float finalChance = Mathf.Clamp01(selectedDrop.baseChance + pityBonus);

        if (UnityEngine.Random.value < finalChance)
        {
            TriggerDrop(selectedDrop);
        }
        else
        {
            // Increase pity counter if the drop failed
            pityCounters[selectedDrop]++;
            // Save the updated pity counter securely
            PlayerPrefs.SetInt($"PityCounter_{selectedDrop.name}", pityCounters[selectedDrop]);
        }
    }

    private void TriggerDrop(RareDropData drop)
    {
        // Grant the reward via the central RewardManager
        int amount = GetRewardAmount(drop.dropType); // Amount can be configured
        RewardManager.Instance.GrantReward(new Reward($"Rare Drop: {drop.dropType}", ConvertRareDropTypeToRewardType(drop.dropType), amount, drop.dropType.ToString()));

        // Reset pity and track the drop for this run
        pityCounters[drop] = 0;
        PlayerPrefs.SetInt($"PityCounter_{drop.name}", 0); // Save reset pity
        dropsThisRun[drop]++;
        lastDropTime = Time.time;

        // Fire UI event
        OnRareDrop?.Invoke(drop.dropType, amount);
        
        Debug.Log($"<color=yellow>RARE DROP TRIGGERED:</color> {drop.dropType}");
    }

    private RewardType ConvertRareDropTypeToRewardType(RareDropType rareDropType)
    {
        switch (rareDropType)
        {
            case RareDropType.RareChest:
                return RewardType.Chest;
            case RareDropType.GoldenBurst:
                return RewardType.Coins;
            case RareDropType.GemFragment:
                return RewardType.Gems;
            case RareDropType.XPBoost:
                return RewardType.XP;
            case RareDropType.SuperPowerUp:
                return RewardType.SuperPowerUp;
            case RareDropType.CosmeticFragment:
                return RewardType.CosmeticFragment;
            case RareDropType.LeaguePointBoost:
                return RewardType.LeaguePointBoost;
            case RareDropType.MysteryReward:
                return RewardType.MysteryReward;
            default:
                Debug.LogWarning($"No RewardType mapping for {rareDropType}");
                return RewardType.Coins; // Fallback
        }
    }
    
    private int GetRewardAmount(RareDropType dropType)
    {
        // Amounts should be configurable via Remote Config or a balance sheet
        // int amount = RemoteConfig.GetInt($"RareDrop_{dropType}_Amount", 1);
        switch (dropType)
        {
            case RareDropType.GoldenBurst:
                return 500;
            case RareDropType.GemFragment:
                return 10;
            case RareDropType.XPBoost:
                return 100;
            case RareDropType.LeaguePointBoost:
                return 50;
            default:
                return 1;
        }
    }

    // --- Event Handlers for Trigger Conditions ---

    private void OnCoinCollected(int amount) 
    {
        // Example: check every 100 coins
        if (amount % 100 == 0) 
        {
            TryTriggerDrop(1); // Low risk tier
        }
    }

    private void OnBossSurvived()
    {
        TryTriggerDrop(3); // High risk tier
    }

    private void OnComboEnded(int finalCombo)
    {
        if (finalCombo > 20) // Threshold for a significant combo
        {
            TryTriggerDrop(2); // Medium risk tier
        }
    }

    private void OnMilestoneReached(int milestone)
    {
        TryTriggerDrop(1); // Low risk tier
    }
}
