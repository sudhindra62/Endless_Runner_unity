public static class UnlockTypeExtensions
{
    // 🔹 EXISTING — KEPT
    public static UnlockType ToUnlockType(this SkinUnlockType type)
    {
        return (UnlockType)(int)type;
    }

    // 🔹 ADDED — REQUIRED FOR REVERSE COMPARISONS
    public static SkinUnlockType ToSkinUnlockType(this UnlockType type)
    {
        return (SkinUnlockType)(int)type;
    }

    // 🔹 ADDED — SAFE COMPARISON HELPER
    public static bool EqualsUnlock(this SkinUnlockType skinType, UnlockType unlockType)
    {
        return (int)skinType == (int)unlockType;
    }
}
