using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EndlessRunner.Utils;

public class ThemeShopItem : MonoBehaviour
{
    public Image themePreview;
    public TextMeshProUGUI themeNameText;
    public TextMeshProUGUI priceText;
    public Button purchaseButton;
    public Button playButton;
    public Button discountButton; // New button for discount

    private ThemeConfig themeConfig;

    public void Setup(ThemeConfig config)
    {
        themeConfig = config;
        themeNameText.text = config.name;
        themePreview.material = new Material(themePreview.material);
        themePreview.material.mainTexture = config.skybox.GetTexture("_Tex");

        if (ThemeUnlockManager.Instance.IsThemeUnlocked(themeConfig))
        {
            ShowAsUnlocked();
        }
        else
        {
            ShowAsLocked();
        }
    }

    private void ShowAsLocked()
    {
        playButton.gameObject.SetActive(false);
        purchaseButton.gameObject.SetActive(true);
        discountButton.gameObject.SetActive(false);

        switch (themeConfig.unlockType)
        {
            case ThemeUnlockType.GemUnlock:
                priceText.text = themeConfig.gemPrice.ToString() + " GEMS";
                purchaseButton.onClick.AddListener(OnPurchaseWithGems);
                discountButton.gameObject.SetActive(true);
                discountButton.onClick.AddListener(OnPurchaseWithDiscount);
                break;
            case ThemeUnlockType.Premium:
                priceText.text = "PREMIUM";
                purchaseButton.onClick.AddListener(OnPurchasePremium);
                break;
        }
    }

    private void ShowAsUnlocked()
    {
        playButton.gameObject.SetActive(true);
        purchaseButton.gameObject.SetActive(false);
        discountButton.gameObject.SetActive(false);
        priceText.text = "UNLOCKED";
        playButton.onClick.AddListener(OnPlay);
    }

    private void OnPurchaseWithGems()
    {
        if (ThemeUnlockManager.Instance.UnlockThemeWithGems(themeConfig))
        {
            ShowAsUnlocked();
        }
        else
        {
            Debug.Log("Not enough gems!");
        }
    }

    private void OnPurchaseWithDiscount()
    {
        AdManager.Instance.ShowRewardedAd(AdManager.RewardType.ThemeDiscount, () => {
            if (ThemeUnlockManager.Instance.UnlockThemeWithDiscount(themeConfig, 20)) // 20% discount
            {
                ShowAsUnlocked();
            }
            else
            {
                Debug.Log("Not enough gems, even with the discount!");
            }
        });
    }

    private void OnPurchasePremium()
    {
        SubscriptionManager.Instance.Subscribe(30);
        if (ThemeUnlockManager.Instance.IsThemeUnlocked(themeConfig))
        {
            ShowAsUnlocked();
        }
    }

    private void OnPlay()
    {
        ThemeManager.Instance.SetTheme((ThemeManager.Theme)System.Enum.Parse(typeof(ThemeManager.Theme), themeConfig.name));
    }
}
