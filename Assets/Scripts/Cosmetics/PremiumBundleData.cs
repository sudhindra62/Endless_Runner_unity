using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A ScriptableObject that represents a premium package, bundling a skin
/// with associated cosmetic items like trails and pets.
/// </summary>
[CreateAssetMenu(fileName = "New Premium Bundle", menuName = "Shop/Premium Bundle")]
public class PremiumBundleData : ScriptableObject
{
    [Header("Bundle Info")]
    [Tooltip("The unique product ID for the IAP store.")]
    public string productID;
    public string bundleName;

    [Header("Contents")]
    [Tooltip("The main skin prefab included in this bundle.")]
    public GameObject skinPrefab;
    [Tooltip("A list of cosmetic items (trails, pets) included.")]
    public List<CosmeticData> includedCosmetics;

    [Header("Purchase Details")]
    public double price;
    public string currencyCode = "INR";

    [Header("Store Display")]
    public Sprite storeIcon;
    [TextArea] public string description;
}
