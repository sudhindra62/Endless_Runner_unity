using UnityEngine;
using UnityEngine.UI;

public class ThemeShopButton : MonoBehaviour
{
    [SerializeField] private Button themeShopButton;
    [SerializeField] private ThemeShopUI themeShopUI;

    private void Start()
    {
        themeShopButton.onClick.AddListener(ShowThemeShop);
    }

    private void ShowThemeShop()
    {
        themeShopUI.ShowPanel();
    }
}
