
/// <summary>
/// A data class that holds all the relevant information for a single run.
/// </summary>
public class RunSessionData
{
    public int Score { get; set; }
    public int CoinsCollected { get; set; }
    public float Distance { get; set; }
    public bool HasRevived { get; set; }

    public void Reset()
    {
        Score = 0;
        CoinsCollected = 0;
        Distance = 0f;
        HasRevived = false;
    }
}
