public static class SkinDataStringAdapter
{
    public static string AsId(this SkinData data)
    {
        return data != null ? data.name : string.Empty;
    }
}
