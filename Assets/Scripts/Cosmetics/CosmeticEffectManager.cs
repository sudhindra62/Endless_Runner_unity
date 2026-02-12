using UnityEngine;

/// <summary>
/// Manages the instantiation and attachment of cosmetic effects based on the equipped skin.
/// </summary>
public class CosmeticEffectManager : MonoBehaviour
{
    public static CosmeticEffectManager Instance { get; private set; }

    [Tooltip("The transform on the player character where effects should be attached.")]
    public Transform playerEffectAnchor; // This should be assigned to the player instance

    private GameObject currentTrailInstance;
    private GameObject currentPetInstance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SkinManager.OnSkinEquipped += HandleSkinEquipped;
    }

    private void OnDisable()
    {
        SkinManager.OnSkinEquipped -= HandleSkinEquipped;
    }

    private void HandleSkinEquipped(GameObject equippedSkinPrefab)
    {
        // We don't have the instance, but we can get the data from the SkinManager
        SkinData skinData = SkinManager.Instance.GetEquippedSkinData();
        if (skinData != null)
        {
            UpdateCosmeticEffects(skinData);
        }
    }

    /// <summary>
    /// Clears old effects and attaches new ones based on the provided SkinData.
    /// </summary>
    public void UpdateCosmeticEffects(SkinData skinData)
    {
        // Destroy previous effects
        if (currentTrailInstance != null) Destroy(currentTrailInstance);
        if (currentPetInstance != null) Destroy(currentPetInstance);

        if (playerEffectAnchor == null)
        {
            Debug.LogWarning("Player Effect Anchor is not set in CosmeticEffectManager!");
            return;
        }

        // Instantiate new run trail, if assigned
        if (skinData.runTrailPrefab != null)
        {
            currentTrailInstance = Instantiate(skinData.runTrailPrefab, playerEffectAnchor.position, playerEffectAnchor.rotation, playerEffectAnchor);
        }

        // Instantiate new pet, if assigned
        if (skinData.petPrefab != null)
        {
            currentPetInstance = Instantiate(skinData.petPrefab, playerEffectAnchor.position, Quaternion.identity);
            // Optional: Make the pet follow the anchor
            // Add a simple follow script to the pet prefab if needed
        }
    }

    // Call this method when your player character is spawned/instantiated
    public void SetPlayerAnchor(Transform anchor)
    {
        playerEffectAnchor = anchor;
        // Re-apply effects for the newly spawned player
        SkinData equippedSkin = SkinManager.Instance.GetEquippedSkinData();
        if(equippedSkin != null) UpdateCosmeticEffects(equippedSkin);
    }
}
