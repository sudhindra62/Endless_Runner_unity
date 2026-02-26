public static class SkinDatabaseAdapter
{
    public static SkinData CreateConfiguredSkin(
        SkinData original,
        SkinUnlockType unlockType,
        int cost)
    {
        // No mutation of original data
        return original;
    }
}
