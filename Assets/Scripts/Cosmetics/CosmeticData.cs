using UnityEngine;

/// <summary>
/// The type of cosmetic effect.
/// </summary>
public enum CosmeticType { Trail, Pet }

/// <summary>
/// A ScriptableObject representing a single cosmetic item (e.g., a trail or a pet).
/// </summary>
[CreateAssetMenu(fileName = "New Cosmetic", menuName = "Cosmetics/Cosmetic Effect")]
public class CosmeticData : ScriptableObject
{
    [Header("Cosmetic Info")]
    [Tooltip("A unique ID for this cosmetic, e.g., TRAIL_FIRE or PET_ROBOT")]
    public string cosmeticID;
    public string cosmeticName;
    public CosmeticType type;

    [Header("Effect")]
    [Tooltip("The prefab to instantiate for this effect (e.g., a particle system or a pet model).")]
    public GameObject effectPrefab;
}
