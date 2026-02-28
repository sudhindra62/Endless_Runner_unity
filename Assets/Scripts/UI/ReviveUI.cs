using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReviveUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Button reviveWithTokenButton;
    [SerializeField] private Button reviveWithGemsButton;
    [SerializeField] private Button watchAdButton;
    [SerializeField] private Button declineButton;
    [SerializeField] private TextMeshProUGUI gemCostText;
    [SerializeField] private TextMeshProUGUI tokenCountText;
    
    [Header("Auto Decline")]
    [SerializeField] private float autoDeclineTime = 8f;
    private float popupVisibleTimestamp;

    private void OnEnable()
    {
        GameFlowController.OnPauseForDeath += ShowPopup;
        ReviveManager.OnPlayerRevived += HidePopup;
        ReviveManager.OnReviveDeclined += HandleReviveFailed;
    }

    private void OnDisable()
    {
        GameFlowController.OnPauseForDeath -= ShowPopup;
        ReviveManager.OnPlayerRevived -= HidePopup;
        ReviveManager.OnReviveDeclined -= HandleReviveFailed;
    }

    private void Start()
    {
        popupPanel.SetActive(false);
        reviveWithTokenButton.onClick.AddListener(OnReviveWithTokenClicked);
        reviveWithGemsButton.onClick.AddListener(OnReviveWithGemsClicked);
        watchAdButton.onClick.AddListener(OnWatchAdClicked);
        declineButton.onClick.AddListener(OnDeclineClicked);
    }

    private void Update()
    {
        if (popupPanel.activeSelf && Time.unscaledTime > popupVisibleTimestamp + autoDeclineTime)
        {
            OnDeclineClicked();
        }
    }

    public void ShowPopup()
    {
        if (ReviveManager.Instance.hasRevivedThisRun)
        {
            GameFlowController.Instance.EndRunFinal();
            return;
        }

        int tokens = 0; // Placeholder
        reviveWithTokenButton.interactable = tokens > 0;
        if (tokenCountText != null) tokenCountText.text = $"x{tokens}";

        int gemCost = 100; // Placeholder
        bool canAffordGems = true; // Placeholder
        reviveWithGemsButton.interactable = canAffordGems;
        if (gemCostText != null) gemCostText.text = gemCost.ToString();

        bool isAdReady = true; // Placeholder
        watchAdButton.gameObject.SetActive(isAdReady);

        popupPanel.SetActive(true);
        popupVisibleTimestamp = Time.unscaledTime;
    }

    private void HidePopup()
    {
        popupPanel.SetActive(false);
    }

    private void OnReviveWithTokenClicked()
    {
        // Placeholder
    }

    private void OnReviveWithGemsClicked()
    {
        ReviveManager.Instance.AttemptGemRevive();
    }

    private void OnWatchAdClicked()
    {
        // Placeholder
    }

    private void OnDeclineClicked()
    {
        ReviveManager.Instance.DeclineRevive();
        HidePopup();
        GameFlowController.Instance.EndRunFinal();
    }

    private void HandleReviveFailed()
    {
        HidePopup();
        GameFlowController.Instance.EndRunFinal();
    }
}
