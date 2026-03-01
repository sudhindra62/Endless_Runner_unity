
using UnityEngine;

// This is a placeholder to demonstrate integration with World Events.
public class CharacterPassiveManager : MonoBehaviour
{
    public static CharacterPassiveManager Instance { get; private set; }

    // Example: Let's say the active character has a passive 5% XP bonus.
    private const float XP_PASSIVE_BONUS = 1.05f;
    private const string PASSIVE_SOURCE_ID = "CharacterPassive";

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
            return;
        }

        ApplyPassiveBonuses();
    }

    private void ApplyPassiveBonuses()
    {
        // This would be based on the currently selected character.
        // For demonstration, we apply a hardcoded bonus.
        if (DataManager.Instance != null)
        {
            Debug.Log($"Applying Character Passive XP bonus: {XP_PASSIVE_BONUS}x");
            DataManager.Instance.ApplyXpMultiplier(PASSIVE_SOURCE_ID, XP_PASSIVE_BONUS);
        }
    }
}
