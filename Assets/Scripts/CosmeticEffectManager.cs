
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the database of cosmetic effects and the currently equipped effect for the player.
/// This system is the central authority for cosmetic visuals.
/// Fully re-architected by the Supreme Guardian Architect v12.
/// </summary>
public class CosmeticEffectManager : Singleton<CosmeticEffectManager>
{
    [Header("Configuration")]
    [Tooltip("A list of all possible cosmetic effects available in the game.")]
    [SerializeField] private List<CosmeticEffectData> effectDatabase = new List<CosmeticEffectData>();

    [Tooltip("The transform to which equipped effects will be parented. If null, will default to this manager's transform.")]
    [SerializeField] private Transform effectParent;

    // --- PUBLIC PROPERTIES ---
    public CosmeticEffectData CurrentlyEquippedEffect { get; private set; }

    // --- PRIVATE STATE ---
    private GameObject _currentEffectInstance;
    private const string EQUIPPED_EFFECT_SAVE_KEY = "EquippedCosmeticEffectID";

    #region Unity Lifecycle & Initialization

    protected override void Awake()
    {
        base.Awake();
        if (effectParent == null)
        {
            effectParent = transform; // Default to self if no parent is assigned
        }
    }

    private void Start()
    {
        // Load the previously equipped effect on startup
        LoadEquippedEffect();
    }

    #endregion

    #region Public API

    /// <summary>
    /// Equips a cosmetic effect, instantiating its prefab.
    /// Will fail if the effect is not in the database or not unlocked by the player.
    /// </summary>
    /// <param name="effectID">The ID of the effect to equip.</param>
    public void EquipEffect(string effectID)
    {
        // --- DELEGATION_MANDATE: Unequip current effect first ---
        UnequipCurrentEffect();

        if (string.IsNullOrEmpty(effectID))
        {
            Debug.Log("Guardian Architect: EquipEffect called with null or empty ID. Effect has been unequipped.");
            SaveEquippedEffect(null);
            return;
        }

        CosmeticEffectData effectToEquip = GetEffectByID(effectID);

        // --- ERROR_HANDLING_POLICY: Validate effect existence and ownership ---
        if (effectToEquip == null)
        {
            Debug.LogError($"Guardian Architect FATAL_ERROR: Cannot equip effect. ID '{effectID}' not found in the database.");
            return;
        }

        if (!CosmeticInventoryManager.Instance.IsEffectUnlocked(effectID))
        {
            Debug.LogWarning($"Guardian Architect Warning: Cannot equip effect '{effectID}'. It is not unlocked.");
            return;
        }

        // --- RESOURCE_MANAGEMENT_MANDATE: Instantiate and parent the new effect ---
        if (effectToEquip.EffectPrefab != null)
        {
            _currentEffectInstance = Instantiate(effectToEquip.EffectPrefab, effectParent.position, effectParent.rotation, effectParent);
        }

        CurrentlyEquippedEffect = effectToEquip;
        SaveEquippedEffect(effectID);
        Debug.Log($"<color=lime>Guardian Architect: Cosmetic Effect '{effectToEquip.DisplayName}' equipped.</color>");
    }

    /// <summary>
    /// Unequips the currently active cosmetic effect, destroying its instance.
    /// </summary>
    public void UnequipCurrentEffect()
    {
        if (_currentEffectInstance != null)
        {
            Destroy(_currentEffectInstance);
            _currentEffectInstance = null;
        }
        CurrentlyEquippedEffect = null;
        SaveEquippedEffect(null); // Save the unequipped state
    }

    /// <summary>
    /// Retrieves a cosmetic effect's data from the database by its ID.
    /// </summary>
    /// <param name="effectID">The ID of the effect to find.</param>
    /// <returns>The effect data, or null if not found.</returns>
    public CosmeticEffectData GetEffectByID(string effectID)
    {
        return effectDatabase.FirstOrDefault(e => e.EffectID == effectID);
    }

    /// <summary>
    /// Gets the entire database of cosmetic effects.
    /// </summary>
    public IEnumerable<CosmeticEffectData> GetAllEffects()
    {
        return effectDatabase;
    }

    #endregion

    #region Persistence

    private void SaveEquippedEffect(string effectID)
    {
        if (string.IsNullOrEmpty(effectID))
        {
            PlayerPrefs.DeleteKey(EQUIPPED_EFFECT_SAVE_KEY);
        }
        else
        {
            PlayerPrefs.SetString(EQUIPPED_EFFECT_SAVE_KEY, effectID);
        }
        PlayerPrefs.Save();
    }

    private void LoadEquippedEffect()
    {
        string equippedID = PlayerPrefs.GetString(EQUIPPED_EFFECT_SAVE_KEY, null);
        if (!string.IsNullOrEmpty(equippedID))
        {
            // Equip the loaded effect, but only if it's still unlocked
            if (CosmeticInventoryManager.Instance.IsEffectUnlocked(equippedID))
            {
                EquipEffect(equippedID);
            }
            else
            {
                Debug.LogWarning($"Guardian Architect Warning: Saved effect '{equippedID}' is no longer unlocked. Unequipping.");
                UnequipCurrentEffect();
            }
        }
    }

    #endregion
}
