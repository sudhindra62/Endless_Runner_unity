using System;
using UnityEngine;
using UnityEngine.Advertisements;

public static class Advertisement
{
    public static bool IsReady(string placementId = null)
    {
#if UNITY_ADS
        return Advertisement.isInitialized;
#else
        return false;
#endif
    }

    public static void Show(string placementId = null, ShowOptions options = null)
    {
#if UNITY_ADS
        if (Advertisement.isInitialized)
        {
            UnityEngine.Advertisements.Advertisement.Show();
        }
#endif
    }
}

public class ShowOptions
{
    public Action<ShowResult> resultCallback;
}

public enum ShowResult
{
    Finished,
    Skipped,
    Failed
}
