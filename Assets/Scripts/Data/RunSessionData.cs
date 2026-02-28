
/// <summary>
/// A plain data class to hold all the relevant information about the current run.
/// </summary>
public class RunSessionData
{
    public int score;
    public float time;
    public bool hasRevived;

    public void Reset()
    {
        score = 0;
        time = 0f;
        hasRevived = false;
    }
}
