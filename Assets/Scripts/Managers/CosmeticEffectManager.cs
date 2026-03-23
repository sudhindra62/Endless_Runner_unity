using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages cosmetics, their loading, equipping, and runtime instantiation.
/// Global scope.
/// </summary>
public class CosmeticEffectManager : Singleton<CosmeticEffectManager>
{
    [Header("Configuration")]
    [SerializeField] private List<CosmeticEffectData> effectDatabase = new List<CosmeticEffectData>();
    [SerializeField] private Transform playerCharacter;

    public CosmeticEffectData CurrentlyEquippedEffect { get; private set; }
    private GameObject _currentEffectInstance;
    private const string EQUIPPED_EFFECT_SAVE_KEY = "EquippedCosmeticEffectID";

    private Dictionary<CosmeticEffectType, GameObject> activeEffects = new Dictionary<CosmeticEffectType, GameObject>();

    public void UnlockEffect(string effectID)
    {
        if (string.IsNullOrEmpty(effectID)) return;
        if (SaveManager.Instance != null && !SaveManager.Instance.Data.unlockedCosmeticEffects.Contains(effectID))
        {
            SaveManager.Instance.Data.unlockedCosmeticEffects.Add(effectID);
            SaveManager.Instance.SaveGame();
            Debug.Log($"[CosmeticEffectManager] Unlocked effect: {effectID}");
        }
    }

    public void UnlockCosmeticEffect(string effectID) => UnlockEffect(effectID);

    protected override void Awake()
    {
        base.Awake();
        LoadEquippedEffect();
    }

    public void EquipEffect(string effectID)
    {
        UnequipCurrentEffect();

        if (string.IsNullOrEmpty(effectID)) return;

        CosmeticEffectData effectToEquip = GetEffectByID(effectID);
        if (effectToEquip == null) return;

        if (effectToEquip.EffectPrefab != null && playerCharacter != null)
        {
            _currentEffectInstance = Instantiate(effectToEquip.EffectPrefab, playerCharacter.position, playerCharacter.rotation, playerCharacter);
        }

        CurrentlyEquippedEffect = effectToEquip;
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.Data.activeCosmeticID = effectID;
            SaveManager.Instance.SaveGame();
        }
    }

    public void UnequipCurrentEffect()
    {
        if (_currentEffectInstance != null)
        {
            Destroy(_currentEffectInstance);
            _currentEffectInstance = null;
        }
        CurrentlyEquippedEffect = null;
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.Data.activeCosmeticID = "";
            SaveManager.Instance.SaveGame();
        }
    }

    public CosmeticEffectData GetEffectByID(string effectID)
    {
        return effectDatabase.FirstOrDefault(e => e.EffectID == effectID);
    }

    public List<CosmeticEffectData> GetAllEffects()
    {
        return new List<CosmeticEffectData>(effectDatabase);
    }

    public void TriggerPickupEffect(Vector3 position, CosmeticEffectType type)
    {
        // Procedural triggering logic for SFX/VFX
    }

    private void LoadEquippedEffect()
    {
        if (SaveManager.Instance == null) return;
        string equippedID = SaveManager.Instance.Data.activeCosmeticID;
        if (!string.IsNullOrEmpty(equippedID)) EquipEffect(equippedID);
    }
}
