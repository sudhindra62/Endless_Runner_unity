using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the UI popup that gives the player a choice to revive upon death.
/// </summary>
public class ReviveUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject revivePanel;
    [SerializeField] private Button reviveWithGemsButton;
    [SerializeField] private Button reviveWithAdButton;
    [SerializeField] private Button declineButton;
    [SerializeField] private TextMeshProUGUI reviveCostText;
    [SerializeField] private float autoDeclineTime = 5f; // Time before auto-declining.

    private float visibleTime = 0f;

    #region Unity Lifecycle & Event Subscriptions

    private void OnEnable()
    {
        LifeLineManager.OnLifeLineTriggered += ShowRevivePanel;
    }

    private void OnDisable()
    {
        LifeLineManager.OnLifeLineTriggered -= ShowRevivePanel;
    }

    private void Start()
    {
        // Hide panel at start, and wire up button listeners.
        revivePanel.SetActive(false);
        reviveWithGemsButton.onClick.AddListener(OnReviveWithGems);
        reviveWithAdButton.onClick.AddListener(OnReviveWithAd);
        declineButton.onClick.AddListener(OnDecline);
    }

    private void Update()
    {
        // If the panel is visible, check if it's time to auto-decline.
        if (revivePanel.activeSelf && Time.unscaledTime > visibleTime + autoDeclineTime)
        {
            OnDecline();
        }
    }

    #endregion

    private void ShowRevivePanel()
    {
        // In a real game, you'd fetch the cost from a game economy manager.
        reviveCostText.text = "-100 Gems"; // Example cost.

        revivePanel.SetActive(true);
        visibleTime = Time.unscaledTime; // Use unscaled time because Time.timeScale is 0.
    }

    private void HideRevivePanel()
    {
        revivePanel.SetActive(false);
    }

    #region Button Handlers

    private void OnReviveWithGems()
    {
        // In a real game, you would check if the player has enough gems.
        Debug.Log("Attempting to revive with gems...");
        HideRevivePanel();
        LifeLineManager.Instance.UseRevive();
    }

    private void OnReviveWithAd()
    {
        // This would trigger an ad service.
        Debug.Log("Attempting to revive with an ad...");
        HideRevivePanel();
        LifeLineManager.Instance.UseRevive(); // For now, we grant it directly.
    }

    private void OnDecline()
    {
        Debug.Log("Player declined to revive.");
        HideRevivePanel();
        LifeLineManager.Instance.ConfirmGameOver();
    }

    #endregion
}
