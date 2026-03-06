
using UnityEngine;

public enum CollectionItemType
{
    CharacterFragment,
    SkinFragment,
    CosmeticFragment,
    EventCollectible,
    SpecialArtifact
}

[CreateAssetMenu(fileName = "CollectionItemData", menuName = "Collection/Collection Item Data")]
public class CollectionItemData : ScriptableObject
{
    public string itemName;
    public CollectionItemType itemType;
    public int requiredFragments;
    public GameObject rewardPrefab; // The skin, cosmetic, or item to be unlocked
    public Sprite itemIcon;
}
