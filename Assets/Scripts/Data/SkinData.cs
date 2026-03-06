
using UnityEngine;

public enum CurrencyType { Coins, Gems }

[CreateAssetMenu(fileName = "NewSkin", menuName = "Gameplay/Skins/New Skin")]
public class SkinData : ScriptableObject
{
    [Header("Skin Details")]
    public string skinName;
    public string skinID; // Must be unique
    public GameObject playerPrefab;
    public Sprite skinIcon;

    [Header("Purchase Details")]
    public bool isDefault;
    public bool isPremium;
    public CurrencyType currencyType;
    public int price;
}
