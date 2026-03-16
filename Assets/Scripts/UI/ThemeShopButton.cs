using UnityEngine;
using UnityEngine.UI;

public class ThemeShopButton : MonoBehaviour
{
    public Image themeIcon;
    public Text themeName;

    private ThemeSO theme;

    public void Setup(ThemeSO theme)
    {
        this.theme = theme;
        themeIcon.sprite = theme.themeIcon;
        themeName.text = theme.themeName;

        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(SelectTheme);
        }
    }

    private void SelectTheme()
    {
        if (ThemeManager.Instance != null)
        {
            int themeIndex = System.Array.IndexOf(ThemeManager.Instance.themes, theme);
            ThemeManager.Instance.SetTheme(themeIndex);
        }
    }
}
