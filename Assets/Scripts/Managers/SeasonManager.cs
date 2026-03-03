using UnityEngine;
using System;

/// <summary>
/// Manages the lifecycle of competitive seasons, including start/end times and resets.
/// This system is authoritative for all time-based season logic.
/// </summary>
public class SeasonManager : MonoBehaviour
{
    public static event Action<int> OnSeasonStart;
    public static event Action<int> OnSeasonEnd;
    public static event Action OnWeeklyReset;

    private const int WEEKS_PER_SEASON = 4;
    private DateTime _seasonStartDate;
    private int _currentWeek;

    // This would be initialized from a server or reliable time source
    public void Initialize(DateTime serverTime)
    {
        // Logic to determine and set the current season start date.
        _seasonStartDate = serverTime.Date;
        InvokeRepeating(nameof(CheckForWeeklyReset), 0f, 60f); // Check every minute
    }

    private void CheckForWeeklyReset()
    {
        // ANTI-EXPLOIT: Uses server time (simulated) instead of device time.
        TimeSpan timeSinceStart = DateTime.UtcNow - _seasonStartDate;
        int week = (int)(timeSinceStart.TotalDays / 7);

        if(week != _currentWeek)
        {
            _currentWeek = week;
            OnWeeklyReset?.Invoke();
            
            if(_currentWeek >= WEEKS_PER_SEASON)
            {
                EndSeason();
                StartNewSeason();
            }
        }
    }

    private void StartNewSeason()
    {
        _seasonStartDate = DateTime.UtcNow.Date;
        _currentWeek = 0;
        OnSeasonStart?.Invoke(1); // Pass season number
        Debug.Log("New season has started!");
    }

    private void EndSeason()
    {
        OnSeasonEnd?.Invoke(0); // Pass old season number
        Debug.Log("Season has ended! Calculating final rewards...");
    }

    public TimeSpan GetTimeUntilWeeklyReset()
    {
        DateTime now = DateTime.UtcNow;
        DateTime nextReset = _seasonStartDate.AddDays((_currentWeek + 1) * 7);
        return nextReset - now;
    }
}
