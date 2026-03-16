using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThemeButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI themeNameText;
    [SerializeField] private Image themePreviewImage;
    [SerializeField] private Button button;

    private int themeIndex;

    public void Setup(ThemeSO theme, int index)
    {
        themeNameText.text = theme.themeName;
        themePreviewImage.sprite = theme.uiPanelSprite; // Use the panel sprite as a preview
        themeIndex = index;

        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        ThemeManager.Instance.SetTheme(themeIndex);
    }
}
