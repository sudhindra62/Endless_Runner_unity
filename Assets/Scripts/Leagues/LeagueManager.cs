
using UnityEngine;

public class LeagueManager : MonoBehaviour
{
    public static LeagueManager Instance { get; private set; }

    private float leaguePointsMultiplier = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ApplyLeaguePointsMultiplier(float multiplier)
    {
        leaguePointsMultiplier = multiplier;
        Debug.Log($"League points multiplier set to: {multiplier}");
    }

    public void RemoveLeaguePointsMultiplier()
    {
        leaguePointsMultiplier = 1f;
        Debug.Log("League points multiplier reset.");
    }

    // This would be called when a player earns league points
    public int CalculateLeaguePoints(int basePoints)
    {
        return (int)(basePoints * leaguePointsMultiplier);
    }
}
