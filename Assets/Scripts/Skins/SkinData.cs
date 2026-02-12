using UnityEngine;

[System.Serializable]
public partial class SkinData : ScriptableObject
{
    // =========================
    // ORIGINAL STORED DATA
    // =========================

    [SerializeField] private string skinId;
    [SerializeField] private string skinName;
    [SerializeField] private SkinUnlockType skinUnlockType;
    [SerializeField] private int skinCost; // 🔹 RENAMED (field only)

    // =========================
    // ORIGINAL READ-ONLY API (KEPT)
    // =========================

    public string Id => skinId;

    // 🔹 KEEP these public APIs — but ensure SINGLE source
    public SkinUnlockType UnlockType => skinUnlockType;
    public int Cost => skinCost;

    // =========================
    // 🔹 ADDITIVE COMPATIBILITY ALIAS
    // =========================

    // READ-ONLY — legacy systems
    public UnlockType UnlockTypeCompat => skinUnlockType.ToUnlockType();

    // =========================
    // 🔹 WRITE-SAFE INTERNAL SETTERS
    // =========================

    public void SetUnlockType(SkinUnlockType type)
    {
        skinUnlockType = type;
    }

    public void SetCost(int value)
    {
        skinCost = value;
    }

    public void SetName(string name)
    {
        skinName = name;
    }

    public void SetId(string id)
    {
        skinId = id;
    }
}
