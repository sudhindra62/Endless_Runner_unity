using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controls the UI popup that offers the player a chance to revive.
/// </summary>
public class RevivePopupUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private Button reviveWithTokenButton;
    [SerializeField] private Button reviveWithGemsButton;
    [SerializeField] private Button watchAdButton;
    [SerializeField] private Button declineButton;
    [SerializeField] private TextMeshProUGUI gemCostText;
    [SerializeField] private TextMeshProUGUI tokenCountText;

    private AdReviveHandler adReviveHandler;

    private void Awake()
    {
        adReviveHandler = gameObject.AddComponent<AdReviveHandler>();
    }

    private void OnEnable()
    {
        GameFlowController.OnPauseForDeath += ShowPopup;
        ReviveManager.OnReviveSuccess += HidePopup;
        ReviveManager.OnReviveFailed += HandleReviveFailed; 
    }

    private void OnDisable()
    {
        GameFlowController.OnPauseForDeath -= ShowPopup;
        ReviveManager.OnReviveSuccess -= HidePopup;
        ReviveManager.OnReviveFailed -= HandleReviveFailed;
    }

    private void Start()
    {
        // Ensure popup is hidden at the start
        popupPanel.SetActive(false);

        // Hook up button listeners
        reviveWithTokenButton.onClick.AddListener(OnReviveWithTokenClicked);
        reviveWithGemsButton.onClick.AddListener(OnReviveWithGemsClicked);
        watchAdButton.onClick.AddListener(OnWatchAdClicked);
        declineButton.onClick.AddListener(OnDeclineClicked);
    }

    public void ShowPopup()
    {
        if (!ReviveManager.Instance.CanRevive())
        {
            // Immediately proceed to game over if no revives are left
            GameFlowController.Instance.EndRunFinal();
            return; 
        }

        // Configure buttons based on available resources
        int tokens = (ReviveTokenManager.Instance != null) ? ReviveTokenManager.Instance.GetTokenCount() : 0;
        reviveWithTokenButton.interactable = tokens > 0;
        if(tokenCountText != null) tokenCountText.text = $"x{tokens}";

        int gemCost = ReviveManager.Instance.GetReviveGemCost();
        bool canAffordGems = (CurrencyManager.Instance != null) && CurrencyManager.Instance.CanAfford(gemCost, "gems");
        reviveWithGemsButton.interactable = canAffordGems;
        if (gemCostText != null) gemCostText.text = gemCost.ToString();

        // Configure ad button
        bool isAdReady = RewardedAdManager.Instance != null && RewardedAdManager.Instance.IsAdReady();
        watchAdButton.gameObject.SetActive(isAdReady);

        popupPanel.SetActive(true);
    }

    private void HidePopup()
    {
        popupPanel.SetActive(false);
    }

    private void OnReviveWithTokenClicked()
    {
        ReviveManager.Instance.ReviveWithToken();
    }

    private void OnReviveWithGemsClicked()
    {
        ReviveManager.Instance.ReviveWithGems();
    }

    private void OnWatchAdClicked()
    {
        adReviveHandler.OnWatchAdToRevive();
    }

    private void OnDeclineClicked()
    {
        Debug.Log("Player declined revive.");
        HidePopup();
        GameFlowController.Instance.EndRunFinal();
    }

    private void HandleReviveFailed()
    {
        HidePopup();
        GameFlowController.Instance.EndRunFinal();
    }
}
