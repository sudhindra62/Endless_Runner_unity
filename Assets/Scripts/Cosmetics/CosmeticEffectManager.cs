
using UnityEngine;
using System.Collections.Generic;

public class CosmeticEffectManager : Singleton<CosmeticEffectManager>
{
    private Dictionary<CosmeticEffectType, GameObject> activeEffects = new Dictionary<CosmeticEffectType, GameObject>();
    private Dictionary<string, ObjectPool> effectPools = new Dictionary<string, ObjectPool>();
    private HashSet<string> unlockedEffects = new HashSet<string>();

    // This would be linked to the player character in the scene
    public Transform playerCharacter;

    protected override void Awake()
    {
        base.Awake();
        LoadUnlockedEffects();
    }

    public void OnCosmeticEquipped(string effectID, CosmeticEffectType effectType)
    {
        // Deactivate any existing effect of this type
        if (activeEffects.ContainsKey(effectType) && activeEffects[effectType] != null)
        {
            activeEffects[effectType].SetActive(false);
        }

        // Activate the new effect
        CosmeticEffectData effectData = GetCosmeticData(effectID); // Assume this method exists to get data
        if (effectData != null && IsEffectUnlocked(effectID))
        {
            GameObject effectInstance = GetPooledEffect(effectID, effectData.effectPrefab);
            effectInstance.transform.SetParent(playerCharacter, false);
            effectInstance.SetActive(true);
            activeEffects[effectType] = effectInstance;
        }
    }

    public void TriggerCoinPickupEffect(Vector3 position)
    {
        string effectID = CosmeticInventoryManager.Instance.GetEquippedCosmetic(CosmeticEffectType.CoinPickup);
        if (effectID != null)
        {
            CosmeticEffectData effectData = GetCosmeticData(effectID);
            if (effectData != null)
            {
                GameObject effect = GetPooledEffect(effectID, effectData.effectPrefab);
                effect.transform.position = position;
                effect.SetActive(true);
                // The effect should have a script to disable itself after playing
            }
        }
    }

    public void UnlockEffect(string effectID)
    {
        if (unlockedEffects.Add(effectID))
        {
            Debug.Log($"Cosmetic Effect Unlocked: {effectID}");
            SaveUnlockedEffects();
        }
    }

    public bool IsEffectUnlocked(string effectID)
    {
        return unlockedEffects.Contains(effectID);
    }

    private void LoadUnlockedEffects()
    {
        // In a real implementation, this would load from a SaveManager
        // For this example, we'll just initialize an empty set.
        unlockedEffects = new HashSet<string>();
    }

    private void SaveUnlockedEffects()
    {
        // In a real implementation, this would save to a SaveManager
    }

    private GameObject GetPooledEffect(string effectID, GameObject prefab)
    {
        if (!effectPools.ContainsKey(effectID))
        {
            effectPools[effectID] = new ObjectPool(prefab);
        }
        return effectPools[effectID].Get();
    }

    private CosmeticEffectData GetCosmeticData(string effectID)
    {
        // In a real implementation, this would fetch the data from a central repository
        // of all cosmetic items, likely loaded from a ScriptableObject or JSON config.
        // For this example, we'll return a dummy data object.
        return new CosmeticEffectData { effectID = effectID, effectPrefab = new GameObject("dummy_effect") };
    }
}

public class ObjectPool
{
    private GameObject prefab;
    private List<GameObject> pooledObjects = new List<GameObject>();

    public ObjectPool(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public GameObject Get()
    {
        foreach (var obj in pooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }

        GameObject newObj = MonoBehaviour.Instantiate(prefab);
        pooledObjects.Add(newObj);
        return newObj;
    }
}
