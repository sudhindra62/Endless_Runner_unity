/// <summary>
/// Defines the method by which a skin can be unlocked.
/// This enum determines the currency or action required to acquire a skin.
/// </summary>
public enum SkinUnlockType
{
    Free,
    Coins,
    Gems,
    Reward,
    Ad,

    // 🔹 COMPATIBILITY VALUE (REQUIRED BY LEGACY CODE)
    Paid
}
