using UnityEngine;
using UnityEngine.UI;

public class ThemeButtonUI : MonoBehaviour
{
    [SerializeField] private Button themeButton;
    private int themeIndex;

    private void Start()
    {
        themeButton.onClick.AddListener(OnButtonClick);
    }

    public void Setup(int index)
    {
        themeIndex = index;
        // Set button appearance based on theme data
    }

    private void OnButtonClick()
    {
        ThemeManager.Instance.SetTheme(themeIndex);
    }
}
