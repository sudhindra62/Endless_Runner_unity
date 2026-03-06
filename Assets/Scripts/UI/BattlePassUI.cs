
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

/// <summary>
/// Manages the UI for the Battle Pass screen, displaying tiers and progress.
/// </summary>
public class BattlePassUI : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private BattlePassManager battlePassManager;

    [Header("UI Elements")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text currentTierText;
    [SerializeField] private GameObject tierItemPrefab;
    [SerializeField] private Transform tierContainer;

    private List<BattlePassTierUI> tierItems = new List<BattlePassTierUI>();

    private void Start()
    {
        if (battlePassManager == null) battlePassManager = BattlePassManager.Instance;
        GenerateTiers();
        SubscribeToEvents();
        RefreshAll();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void SubscribeToEvents()
    {
        BattlePassManager.OnXPChanged += RefreshXP;
        BattlePassManager.OnTierChanged += (tier) => RefreshAll();
    }

    private void UnsubscribeFromEvents()
    {
        BattlePassManager.OnXPChanged -= RefreshXP;
        BattlePassManager.OnTierChanged -= (tier) => RefreshAll();
    }

    private void GenerateTiers()
    {
        BattlePassData seasonData = battlePassManager.GetSeasonData();
        for (int i = 0; i < seasonData.tiers.Count; i++)
        {
            GameObject tierGO = Instantiate(tierItemPrefab, tierContainer);
            BattlePassTierUI tierUI = tierGO.GetComponent<BattlePassTierUI>();
            tierUI.Setup(i, seasonData.tiers[i]);
            tierItems.Add(tierUI);
        }
    }

    private void RefreshAll()
    {
        RefreshXP(battlePassManager.GetCurrentXP(), battlePassManager.GetCurrentTierMaxXP());
        currentTierText.text = $"Tier {battlePassManager.GetCurrentTier() + 1}";
        foreach (var item in tierItems)
        {
            item.Refresh();
        }
    }

    private void RefreshXP(int current, int max)
    {
        if (max > 0)
        {
            xpSlider.value = (float)current / max;
            xpText.text = $"{current} / {max} XP";
        }
        else // Max tier
        {
            xpSlider.value = 1f;
            xpText.text = "MAX TIER";
        }
    }
}
