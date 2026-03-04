using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    private int currentCombo = 0;
    private int comboPeak = 0;

    private PlayerAnalyticsManager analyticsManager;

    private void Start()
    {
        analyticsManager = PlayerAnalyticsManager.Instance;
    }

    public void IncrementCombo()
    {
        currentCombo++;
        if (currentCombo > comboPeak)
        {
            comboPeak = currentCombo;
            if (analyticsManager != null)
            {
                analyticsManager.LogComboPeak(comboPeak);
            }
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
    }
}
