using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays a banner on the home screen for the active Live Event.
/// Subscribes to LiveEventManager events and does not poll.
/// </summary>
public class EventBannerUI : MonoBehaviour
{
    [SerializeField] private GameObject bannerRoot;
    [SerializeField] private Text eventNameText;

    private void OnEnable()
    {
        LiveEventManager.OnEventStarted += ShowBanner;
        LiveEventManager.OnEventEnded += HideBanner;
    }

    private void OnDisable()
    {
        LiveEventManager.OnEventStarted -= ShowBanner;
        LiveEventManager.OnEventEnded -= HideBanner;
    }

    private void ShowBanner(LiveEvent liveEvent)
    {
        if (liveEvent == null) return;
        eventNameText.text = liveEvent.eventName;
        bannerRoot.SetActive(true);
    }

    private void HideBanner(LiveEvent liveEvent)
    {
        bannerRoot.SetActive(false);
    }
}
