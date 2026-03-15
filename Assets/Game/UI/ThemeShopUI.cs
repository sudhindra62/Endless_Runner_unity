using UnityEngine;
using UnityEngine.UI;

public class ThemeShopUI : MonoBehaviour
{
    [SerializeField] private GameObject themeShopPanel;
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform themeButtonsParent;
    [SerializeField] private GameObject themeButtonPrefab;

    private void Start()
    {
        closeButton.onClick.AddListener(HidePanel);
    }

    public void ShowPanel()
    {
        themeShopPanel.SetActive(true);
        PopulateThemeButtons();
    }

    public void HidePanel()
    {
        themeShopPanel.SetActive(false);
    }

    private void PopulateThemeButtons()
    {
        // Clear existing buttons
        foreach (Transform child in themeButtonsParent)
        {
            Destroy(child.gameObject);
        }

        // Populate with themes from ThemeManager
    }
}
