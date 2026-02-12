/// <summary>
/// COMPATIBILITY EXTENSIONS FOR PlayerMetaData
/// 
/// This file exists ONLY to satisfy legacy references.
/// No gameplay logic is changed or duplicated.
/// </summary>
public partial class PlayerMetaData
{
    // 🔹 Legacy field aliases (read-only)
    public int totalCoins => TotalCoins;
    public int totalGems  => TotalGems;

    // 🔹 Legacy removal APIs (safe wrappers)
    public void RemoveCoins(int amount)
    {
        if (amount <= 0) return;
        AddCoins(-amount);
    }

    public void RemoveGems(int amount)
    {
        if (amount <= 0) return;
        AddGems(-amount);
    }
}
