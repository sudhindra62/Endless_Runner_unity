
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CosmeticsPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject cosmeticsPanel;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform trailsTabContainer;
    [SerializeField] private Transform aurasTabContainer;
    [SerializeField] private Transform coinEffectsTabContainer;
    [SerializeField] private Transform footstepEffectsTabContainer;
    [SerializeField] private GameObject cosmeticItemUIPrefab; // Prefab for a single cosmetic item in the UI

    void Start()
    {
        openButton.onClick.AddListener(() => cosmeticsPanel.SetActive(true));
        closeButton.onClick.AddListener(() => cosmeticsPanel.SetActive(false));
        cosmeticsPanel.SetActive(false);
        PopulateCosmeticsTabs();
    }

    private void PopulateCosmeticsTabs()
    {
        // In a real game, you would get this list from a central game data manager
        List<CosmeticEffectData> allCosmetics = GetAllCosmeticData();

        foreach (var cosmetic in allCosmetics)
        {
            Transform parentContainer = GetContainerForType(cosmetic.effectType);
            if (parentContainer != null)
            {
                GameObject itemUI = Instantiate(cosmeticItemUIPrefab, parentContainer);
                // Configure the itemUI with cosmetic details (name, icon, unlock status, etc.)
                // Add a button listener to equip the item.
            }
        }
    }

    private Transform GetContainerForType(CosmeticEffectType type)
    {
        switch (type)
        {
            case CosmeticEffectType.Trail: return trailsTabContainer;
            case CosmeticEffectType.CharacterAura: return aurasTabContainer;
            case CosmeticEffectType.CoinPickup: return coinEffectsTabContainer;
            case CosmeticEffectType.Footstep: return footstepEffectsTabContainer;
            default: return null;
        }
    }

    private List<CosmeticEffectData> GetAllCosmeticData()
    {
        if (CosmeticEffectManager.Instance != null)
        {
            return CosmeticEffectManager.Instance.GetAllEffects();
        }

        return new List<CosmeticEffectData>();
    }
}
