using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Central hub for rare drop logic, weight-based selection, and pity systems.
/// Integrated with IntegrityManager for anti-exploit validation.
/// Global scope Singleton.
/// </summary>
public class RareDropManager : Singleton<RareDropManager>
{
    [SerializeField] private List<RareDropData> rareDropPool;
    public static event Action<RareDropType, int> OnRareDrop;
    public static event Action<RareDropData> OnRareDropAwarded; // Legacy support

    private Dictionary<RareDropData, int> pityCounters = new Dictionary<RareDropData, int>();
    private Dictionary<RareDropData, int> dropsThisRun = new Dictionary<RareDropData, int>();
    private float lastDropTime = -Mathf.Infinity;
    private const float DROP_COOLDOWN = 1.0f;

    protected override void Awake()
    {
        base.Awake();
        InitializeDrops();
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        GameEvents.OnCoinCollected += OnCoinCollected;
        BossChaseManager.OnBossChaseEnd += OnBossSurvived;
        FlowComboManager.OnComboBroken += OnComboEnded;
        MilestoneManager.OnMilestoneReached += OnMilestoneReached;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void UnsubscribeFromEvents()
    {
        GameEvents.OnCoinCollected -= OnCoinCollected;
        BossChaseManager.OnBossChaseEnd -= OnBossSurvived;
        FlowComboManager.OnComboBroken -= OnComboEnded;
        MilestoneManager.OnMilestoneReached -= OnMilestoneReached;
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void InitializeDrops()
    {
        if (rareDropPool == null) return;
        foreach (RareDropData drop in rareDropPool)
        {
            if (SaveManager.Instance != null && SaveManager.Instance.Data.rareDropPityCounters.TryGetValue(drop.name, out int count))
            {
                pityCounters[drop] = count;
            }
            else
            {
                pityCounters[drop] = 0;
            }
            dropsThisRun[drop] = 0;
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing) ResetRunTracking();
    }

    private void ResetRunTracking()
    {
        foreach (var key in dropsThisRun.Keys.ToList()) dropsThisRun[key] = 0;
    }

    public void EvaluateRareDrop(RunSessionData runData, bool bossDefeated)
    {
        // Automated roll logic for end-of-run rewards
        AwardRandomDrop();
    }

    public void TryTriggerDrop(int riskTier)
    {
        if (Time.time < lastDropTime + DROP_COOLDOWN) return;

        List<RareDropData> eligibleDrops = rareDropPool
            .Where(drop => drop.isEnabled && drop.riskTier <= riskTier && dropsThisRun[drop] < drop.maxPerRun)
            .ToList();

        if (eligibleDrops.Count == 0) return;

        float totalWeight = eligibleDrops.Sum(drop => drop.weight);
        float randomRoll = UnityEngine.Random.Range(0, totalWeight);
        
        RareDropData selectedDrop = null;
        float cumulativeWeight = 0f;
        foreach (var drop in eligibleDrops)
        {
            cumulativeWeight += drop.weight;
            if (randomRoll < cumulativeWeight) { selectedDrop = drop; break; }
        }

        if (selectedDrop == null) return;

        // Integrity Check
        if (IntegrityManager.Instance != null && !IntegrityManager.Instance.economyValidator.ValidateRareDrop(selectedDrop, pityCounters[selectedDrop]))
        {
            IntegrityManager.Instance.ReportError("Rare drop validation failed.");
            return;
        }

        float pityBonus = (pityCounters[selectedDrop] >= selectedDrop.pityThreshold) 
            ? (pityCounters[selectedDrop] - selectedDrop.pityThreshold) * 0.01f : 0f;
        
        if (UnityEngine.Random.value < (selectedDrop.baseChance + pityBonus))
        {
            AwardDrop(selectedDrop);
        }
        else
        {
            pityCounters[selectedDrop]++;
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.Data.rareDropPityCounters[selectedDrop.name] = pityCounters[selectedDrop];
                SaveManager.Instance.SaveGame();
            }
        }
    }

    private void AwardDrop(RareDropData drop)
    {
        int amount = GetRewardAmount(drop.dropType);
        if (RewardManager.Instance != null)
        {
            RewardManager.Instance.GrantReward(new Reward($"Rare Drop", ConvertType(drop.dropType), amount, drop.itemID));
        }

        pityCounters[drop] = 0;
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.Data.rareDropPityCounters[drop.name] = 0;
            SaveManager.Instance.SaveGame();
        }
        dropsThisRun[drop]++;
        lastDropTime = Time.time;

        OnRareDrop?.Invoke(drop.dropType, amount);
        OnRareDropAwarded?.Invoke(drop);
    }

    public void AwardRandomDrop() => TryTriggerDrop(3);

    private RewardType ConvertType(RareDropType type)
    {
        switch (type) {
            case RareDropType.RareChest: return RewardType.Chest;
            case RareDropType.GoldenBurst: return RewardType.Coins;
            case RareDropType.GemFragment: return RewardType.Gems;
            default: return RewardType.Coins;
        }
    }

    private int GetRewardAmount(RareDropType type) => 1;

    private void OnCoinCollected(int amount) { if (amount % 100 == 0) TryTriggerDrop(1); }
    private void OnBossSurvived() => TryTriggerDrop(3);
    private void OnComboEnded(int finalCombo) { if (finalCombo > 20) TryTriggerDrop(2); }
    private void OnMilestoneReached(int milestone) => TryTriggerDrop(1);
}
