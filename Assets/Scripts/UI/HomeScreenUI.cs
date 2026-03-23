
using UnityEngine;
using UnityEngine.UI;
using TMPro;

    public class HomeScreenUI : MonoBehaviour
    {
        [Header("Main Buttons")]
        public Button characterButton;
        public Button themeShopButton;
        public Button settingsButton;
        public Button dailyRewardsButton;
        public Button soundToggleButton;
        public Button tapToRunButton; // Changed from GameObject

        [Header("UI Panels")]
        public GameObject characterPanel;
        public GameObject themeShopPanel;
        public GameObject settingsPanel;
        public GameObject dailyRewardsPanel;

        [Header("Currency Display")]
        public TextMeshProUGUI coinText;
        public TextMeshProUGUI gemText;

        [Header("Animations")]
        public Animator screenAnimator;

        private void Start()
        {
            // Add listeners for the buttons
            characterButton.onClick.AddListener(ShowCharacterPanel);
            themeShopButton.onClick.AddListener(ShowThemeShopPanel);
            settingsButton.onClick.AddListener(ShowSettingsPanel);
            dailyRewardsButton.onClick.AddListener(ShowDailyRewardsPanel);
            soundToggleButton.onClick.AddListener(ToggleSound);
            tapToRunButton.onClick.AddListener(OnTapToRun);

            // Start with the main screen visible
            ShowMainScreen();
        }

        private void OnTapToRun()
        {
            // Find the cinematic manager and play the pre-run cinematic
            if (PreRunCinematicManager.Instance != null)
            {
                PreRunCinematicManager.Instance.PlayCinematic();
                gameObject.SetActive(false); // Hide the home screen
            }
        }

        public void ShowMainScreen()
        {
            gameObject.SetActive(true);
            if(characterPanel) characterPanel.SetActive(false);
            if(themeShopPanel) themeShopPanel.SetActive(false);
            if(settingsPanel) settingsPanel.SetActive(false);
            if(dailyRewardsPanel) dailyRewardsPanel.SetActive(false);

            if (screenAnimator) screenAnimator.Play("HomeScreen_Intro");
        }

        private void ShowCharacterPanel()
        {
            if(characterPanel) characterPanel.SetActive(true);
        }

        private void ShowThemeShopPanel()
        {
            if(themeShopPanel) themeShopPanel.SetActive(true);
        }

        private void ShowSettingsPanel()
        {
            if(settingsPanel) settingsPanel.SetActive(true);
        }

        private void ShowDailyRewardsPanel()
        {
            if(dailyRewardsPanel) dailyRewardsPanel.SetActive(true);
        }

        private void ToggleSound()
        {
            // Logic to toggle sound on and off
        }

        public void UpdateCurrencyDisplay(int coins, int gems)
        {
            coinText.text = coins.ToString();
            gemText.text = gems.ToString();
        }
    }
