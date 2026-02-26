using UnityEngine;

/// <summary>
/// COMPATIBILITY MANAGER ONLY.
/// This class exists to satisfy existing references in UI and revive systems.
/// It does NOT implement ad logic or change runtime behavior.
/// </summary>
public class RewardedAdManager : MonoBehaviour
{
    public static RewardedAdManager Instance { get; private set; }

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

    /// <summary>
    /// Compatibility API expected by RevivePopupUI.
    /// Returns false unless a real ad system overrides this behavior.
    /// </summary>
    public bool IsAdReady()
    {
        return false;
    }

    /// <summary>
    /// Compatibility hook for showing a rewarded ad.
    /// No implementation here by design.
    /// </summary>
    public void ShowAd(System.Action onRewardGranted, System.Action onAdFailed = null)
    {
        onAdFailed?.Invoke();
    }
}
