
using UnityEngine;

public class PityCounterManager : MonoBehaviour
{
    public static PityCounterManager Instance { get; private set; }

    // These would be loaded from SaveManager
    private int epicPityCounter = 0;
    private int legendaryPityCounter = 0;
    private int mythicPityCounter = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // To be called by SaveManager on game load
    public void LoadPityCounters(int epic, int legendary, int mythic)
    {
        epicPityCounter = epic;
        legendaryPityCounter = legendary;
        mythicPityCounter = mythic;
    }

    public void IncrementPityCounters()
    {
        epicPityCounter++;
        legendaryPityCounter++;
        mythicPityCounter++;
        // SaveManager.Instance.SaveGame(); // Save after incrementing
    }

    public float GetPityBoost(string rarity)
    {
        // Example: 100 runs for legendary pity threshold
        const int LEGENDARY_PITY_THRESHOLD = 100;
        const int MYTHIC_PITY_THRESHOLD = 200;

        switch (rarity)
        {
            case "Legendary":
                // Soft boost: increases chance as player approaches the threshold
                return 1.0f + (0.5f * ((float)legendaryPityCounter / LEGENDARY_PITY_THRESHOLD));
            case "Mythic":
                // Strong boost: significant increase as player approaches the threshold
                return 1.0f + (1.0f * ((float)mythicPityCounter / MYTHIC_PITY_THRESHOLD));
            default:
                return 1.0f;
        }
    }

    public bool IsPityGuaranteeMet(string rarity)
    {
        const int EPIC_PITY_THRESHOLD = 30; // Guarantee after 30 runs
        if (rarity == "Epic" && epicPityCounter >= EPIC_PITY_THRESHOLD)
        {
            return true;
        }
        return false;
    }

    public void ResetPityCounter(string rarity)
    {
        switch (rarity)
        {
            case "Epic":
                epicPityCounter = 0;
                break;
            case "Legendary":
                legendaryPityCounter = 0;
                break;
            case "Mythic":
                // Also reset lower tiers upon a higher-tier drop
                epicPityCounter = 0;
                legendaryPityCounter = 0;
                mythicPityCounter = 0;
                break;
        }
       // SaveManager.Instance.SaveGame(); // Save after reset
    }
}
