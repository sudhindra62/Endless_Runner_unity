public partial class CurrencyManager
{
    public int GetCoinBalance() => Coins;
    public int GetGemBalance() => Gems;

    public bool SpendCoins(int amount)
    {
        if (Coins < amount) return false;
        AddCoins(-amount);
        return true;
    }
}
