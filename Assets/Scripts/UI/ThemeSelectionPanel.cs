using UnityEngine;
using UnityEngine.UI;

public class ThemeSelectionPanel : MonoBehaviour
{
    public GameObject themeButtonPrefab;
    public Transform contentPanel;
    public ThemeDatabase themeDatabase;

    void Start()
    {
        if (themeDatabase == null || themeDatabase.themes.Length == 0)
        {
            Debug.LogError("ThemeDatabase is not assigned or is empty!");
            return;
        }

        foreach (ThemeSO theme in themeDatabase.themes)
        {
            GameObject buttonGO = Instantiate(themeButtonPrefab, contentPanel);
            ThemeShopButton button = buttonGO.GetComponent<ThemeShopButton>();
            if (button != null)
            {
                button.Setup(theme);
            }
            else
            {
                Debug.LogError("ThemeButtonPrefab is missing ThemeShopButton component!");
            }
        }
    }
}
