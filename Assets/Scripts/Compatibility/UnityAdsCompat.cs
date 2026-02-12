using System;

namespace UnityEngine.Advertisements
{
    public static class Advertisement
    {
        public static bool IsReady(string placementId = null)
        {
            return false; // SAFE: prevents crash, preserves logic flow
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
}
