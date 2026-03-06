
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Manages the UI panel that displays available rewarded boosts to the player.
/// Dynamically creates and updates boost cards.
/// </summary>
public class RewardedBoostPanelUI : MonoBehaviour
{
    [Header("UI Setup")]
    [SerializeField] private GameObject boostCardPrefab;
    [SerializeField] private Transform cardContainer;

    private void OnEnable()
    {
        PopulateBoosts();
        // You might want to subscribe to an event if boosts can change dynamically
        // RewardedBoostManager.OnAvailableBoostsChanged += PopulateBoosts;
    }

    private void OnDisable()
    {
        // RewardedBoostManager.OnAvailableBoostsChanged -= PopulateBoosts;
    }

    private void PopulateBoosts()
    {
        // Clear existing cards
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        if (RewardedBoostManager.Instance == null) return;

        List<BoostRewardData> boosts = RewardedBoostManager.Instance.availableBoosts;

        foreach (var boostData in boosts)
        {
            GameObject cardGO = Instantiate(boostCardPrefab, cardContainer);
            // Assuming the prefab has a script like BoostCardUI to set its data
            // BoostCardUI cardUI = cardGO.GetComponent<BoostCardUI>();
            // if (cardUI != null)
            // {
            //     cardUI.SetData(boostData, this);
            // }
        }
    }

    public void OnWatchAdButtonPressed(BoostRewardData boostData)
    {
        if (RewardedBoostManager.Instance != null)
        {
            RewardedBoostManager.Instance.RequestAdForBoost(boostData);
        }
    }
}
