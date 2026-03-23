using UnityEngine;

public class TimeControlManager : Singleton<TimeControlManager>
{
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void DoSlowMotion(float scale, float duration)
    {
        // Implementation stub for EffectsManager requirement
    }
}