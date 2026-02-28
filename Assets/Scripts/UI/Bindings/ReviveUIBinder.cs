
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace EndlessRunner.UI.Bindings
{
    public class ReviveUIBinder : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject revivePopup;
        [SerializeField] private Button reviveWithTokenButton;
        [SerializeField] private Button reviveWithGemsButton;
        [SerializeField] private Button watchAdButton;
        [SerializeField] private Button declineButton;
        [SerializeField] private TextMeshProUGUI gemCostText;
        [SerializeField] private TextMeshProUGI tokenCountText;

        [Header("Auto Decline")]
        [SerializeField] private float autoDeclineTime = 8f;
        private float popupVisibleTimestamp;

        private GameStateManager gameStateManager;
        private ReviveManager reviveManager;

        private void Awake()
        {
            revivePopup.SetActive(false);
            reviveWithTokenButton.onClick.AddListener(TokenRevivePressed);
            reviveWithGemsButton.onClick.AddListener(GemRevivePressed);
            watchAdButton.onClick.AddListener(AdRevivePressed);
            declineButton.onClick.AddListener(DeclinePressed);
        }

        private void Start()
        {
            gameStateManager = ServiceLocator.Get<GameStateManager>();
            reviveManager = ServiceLocator.Get<ReviveManager>();
        }

        private void Update()
        {
            if (revivePopup.activeSelf && Time.unscaledTime > popupVisibleTimestamp + autoDeclineTime)
            {
                DeclinePressed();
            }
        }

        public void Show()
        {
            revivePopup.SetActive(true);
            popupVisibleTimestamp = Time.unscaledTime;
        }

        public void Hide()
        {
            revivePopup.SetActive(false);
        }

        public void GemRevivePressed()
        {
            reviveManager?.ReviveWithGems();
            Hide();
        }

        public void AdRevivePressed()
        {
            reviveManager?.ReviveWithAd();
            Hide();
        }

        public void TokenRevivePressed()
        {
            // Assuming a method for token revive exists, if not, this can be implemented
            // reviveManager?.ReviveWithToken(); 
            Hide();
        }

        public void DeclinePressed()
        {
            Hide();
            reviveManager.DeclineRevive();
        }
    }
}
