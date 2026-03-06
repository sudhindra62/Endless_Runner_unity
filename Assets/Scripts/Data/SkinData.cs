using UnityEngine;

public enum CurrencyType { Coins, Gems }
public enum SkinRarity { Common, Rare, Epic, Legendary }
public enum UnlockType { Skin, Item, Character }

[CreateAssetMenu(fileName = "NewSkin", menuName = "Gameplay/Skins/New Skin")]
public class SkinData : ScriptableObject
{
    [Header("Skin Details")]
    public string skinName;
    public string skinID; // Must be unique
    public GameObject playerPrefab;
    public Sprite skinIcon;
    public SkinRarity rarity;
    public UnlockType unlockType;


    [Header("Purchase Details")]
    public bool isDefault;
    public bool isPremium;
    public CurrencyType currencyType;
    public int price;
}
