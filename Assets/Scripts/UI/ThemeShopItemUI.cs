
using UnityEngine;
using UnityEngine.UI;


using TMPro;

    public class ThemeShopItemUI : MonoBehaviour
    {
        public Image themePreview;
        public TextMeshProUGUI themeNameText;
        public TextMeshProUGUI priceText;
        public Button purchaseButton;
        public Button playButton;
        public Button discountButton; // New button for discount ad

        private ThemeSO theme;
        private AnimatedBackground animatedBackground;
        private bool isDiscounted = false;
        private int originalPrice;

        private void Awake()
        {
            animatedBackground = GetComponentInChildren<AnimatedBackground>();
        }

        public void Setup(ThemeSO theme)
        {
            this.theme = theme;
            this.originalPrice = theme.gemPrice;
            
            themePreview.sprite = theme.themeIcon;
            themeNameText.text = theme.themeName;

            if (animatedBackground != null)
            {
                animatedBackground.frames = theme.animatedBackgroundFrames;
            }

            // Check if a discount has been previously applied
            if (SaveManager.Instance != null && SaveManager.Instance.Data.discountedThemes.Contains(theme.themeName))
            {
                isDiscounted = true;
            }

            if (ThemeUnlockManager.Instance.IsThemeUnlocked(theme))
            {
                ShowPlayButton();
            }
            else
            {
                ShowPurchaseButton();
            }
        }

        private void ShowPurchaseButton()
        {
            purchaseButton.gameObject.SetActive(true);
            playButton.gameObject.SetActive(false);
            
            if(discountButton != null)
            {
                discountButton.gameObject.SetActive(false);
            }


            switch (theme.unlockType)
            {
                case ThemeUnlockType.GemUnlock:
                    int currentPrice = isDiscounted ? originalPrice / 2 : originalPrice;
                    priceText.text = currentPrice.ToString() + " Gems";
                    if (!isDiscounted && discountButton != null)
                    {
                        discountButton.gameObject.SetActive(true);
                        discountButton.onClick.RemoveAllListeners();
                        discountButton.onClick.AddListener(OnDiscountAdButtonClicked);
                    }
                    break;
                case ThemeUnlockType.PremiumSubscription:
                    priceText.text = "Premium";
                    break;
                default:
                    priceText.text = "Free";
                    break;
            }

            purchaseButton.onClick.RemoveAllListeners();
            purchaseButton.onClick.AddListener(OnPurchaseButtonClicked);
        }

        private void ShowPlayButton()
        {
            purchaseButton.gameObject.SetActive(false);
            playButton.gameObject.SetActive(true);
            if(discountButton != null)
            {
                discountButton.gameObject.SetActive(false);
            }
            priceText.text = "Unlocked";

            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPurchaseButtonClicked()
        {
            int currentPrice = isDiscounted ? originalPrice / 2 : originalPrice;
            
            // Temporarily set the theme's price to the discounted price for the unlock check
            int actualPrice = theme.gemPrice;
            theme.gemPrice = currentPrice;

            if (ThemeUnlockManager.Instance.UnlockTheme(theme))
            {
                ShowPlayButton();
            }
            
            // Restore original price
            theme.gemPrice = actualPrice;
        }
        
        private void OnDiscountAdButtonClicked()
        {
            AdManager.Instance.ShowRewardedAd(RewardType.ThemeUnlockDiscount, (rewardType) =>
            {
                if (rewardType == RewardType.ThemeUnlockDiscount)
                {
                    if (SaveManager.Instance != null && !SaveManager.Instance.Data.discountedThemes.Contains(theme.themeName))
                    {
                        SaveManager.Instance.Data.discountedThemes.Add(theme.themeName);
                        SaveManager.Instance.SaveGame();
                    }
                    isDiscounted = true;
                    ShowPurchaseButton(); // Refresh the UI
                }
            });
        }

        private void OnPlayButtonClicked()
        {
            ThemeManager.Instance.SetTheme(theme);
            GameManager.Instance.StartGame();
        }
    }

