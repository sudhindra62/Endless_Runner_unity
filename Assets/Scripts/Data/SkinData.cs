
using UnityEngine;

[CreateAssetMenu(fileName = "New Skin", menuName = "Endless Runner/Skin Data")]
public class SkinData : ScriptableObject
{
    [Tooltip("Unique identifier for this skin.")]
    public string skinID;

    [Tooltip("The player prefab with the visual representation of this skin.")]
    public GameObject playerPrefab;

    [Tooltip("The name of the skin to be displayed in the UI.")]
    public string skinName;

    [Tooltip("The description of the skin to be displayed in the UI.")]
    public string skinDescription;

    [Tooltip("The cost of the skin in coins.")]
    public int coinCost;

    [Tooltip("The cost of the skin in gems.")]
    public int gemCost;

    [Tooltip("Is this the default skin? There should only be one.")]
    public bool isDefault = false;
}
