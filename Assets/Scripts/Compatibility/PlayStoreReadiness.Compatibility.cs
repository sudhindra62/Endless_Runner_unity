/// <summary>
/// COMPATIBILITY LAYER for legacy UI access.
/// Does NOT change encapsulation or runtime behavior.
/// </summary>
public partial class PlayStoreReadiness
{
    // 🔹 Legacy static-style accessors (READ-ONLY)
    public static bool adsEnabled
    {
        get => Instance != null && Instance.IsAdsEnabled;
    }

    public static bool iapEnabled
    {
        get => Instance != null && Instance.IsIapEnabled;
    }
}
