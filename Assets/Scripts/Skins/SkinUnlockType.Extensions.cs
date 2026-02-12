public static class SkinUnlockTypeExtensionsSafe
{
    public static UnlockType AsUnlockType(this SkinUnlockType type)
    {
        return (UnlockType)(int)type;
    }
}
