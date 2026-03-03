using UnityEngine;

public class TimeControlManager : Singleton<TimeControlManager>
{
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
}