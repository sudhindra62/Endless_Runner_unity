using UnityEngine;
using UnityEngine.UI;

public class ThemeShopUI : MonoBehaviour
{
    [SerializeField] private GameObject themeButtonPrefab;
    [SerializeField] private Transform themeButtonsParent;

    private void Start()
    {
        // Populate the theme shop with buttons for each theme
        for (int i = 0; i < ThemeManager.Instance.themes.Length; i++)
        {
            GameObject buttonGO = Instantiate(themeButtonPrefab, themeButtonsParent);
            ThemeButtonUI buttonUI = buttonGO.GetComponent<ThemeButtonUI>();
            buttonUI.Setup(ThemeManager.Instance.themes[i], i);
        }
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        gameObject.SetActive(false);
    }
}
