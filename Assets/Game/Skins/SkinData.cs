using UnityEngine;

/// <summary>
/// Defines the data for a single cosmetic skin, including its ID, name, and unlock criteria.
/// This is a ScriptableObject, allowing for easy creation and management of skins in the editor.
/// </summary>
[System.Serializable]
public partial class SkinData : ScriptableObject
{
    [SerializeField]
    private string skinId;

    [SerializeField]
    private string skinName;

    [SerializeField]
    private SkinUnlockType skinUnlockType;

    [SerializeField]
    private int skinCost;

    [SerializeField]
    private GameObject skinPrefab;

    // Public read-only properties to access the skin's data.
    public string Id => skinId;
    public string SkinName => skinName;
    public SkinUnlockType UnlockType => skinUnlockType;
    public int Cost => skinCost;
    public GameObject SkinPrefab => skinPrefab;

    /// <summary>
    /// Sets the unlock type for the skin. This should only be used in controlled contexts,
    /// such as editor scripts or data migration.
    /// </summary>
    public void SetUnlockType(SkinUnlockType type)
    {
        skinUnlockType = type;
    }

    /// <summary>
    /// Sets the cost of the skin. This should only be used in controlled contexts.
    /// </summary>
    public void SetCost(int value)
    {
        skinCost = value;
    }

    /// <summary>
    /// Sets the name of the skin. This should only be used in controlled contexts.
    /// </summary>
    public void SetName(string name)
    {
        skinName = name;
    }

    /// <summary>
    /// Sets the ID of the skin. This should only be used in controlled contexts.
    /// </summary>
    public void SetId(string id)
    {
        skinId = id;
    }

    /// <summary>
    /// Sets the prefab for the skin. This should only be used in controlled contexts.
    /// </summary>
    public void SetPrefab(GameObject prefab)
    {
        skinPrefab = prefab;
    }
}
