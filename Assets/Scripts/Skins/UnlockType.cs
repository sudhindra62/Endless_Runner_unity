/// <summary>
/// Defines how a skin is unlocked.
/// This enum exists only to satisfy existing references.
/// </summary>
public enum UnlockType
{
    Free = 0,
    Coins = 1,
    Gems = 2,
    Reward = 3,
    Ad = 4,

    // 🔹 ADDED — MUST MATCH SkinUnlockType
    Paid = 5
}
