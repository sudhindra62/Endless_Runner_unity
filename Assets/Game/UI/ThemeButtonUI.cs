
using UnityEngine;
using UnityEngine.UI;
using EndlessRunner.Managers;
using EndlessRunner.Themes;

namespace EndlessRunner.UI
{
    public class ThemeButtonUI : MonoBehaviour
    {
        public Text themeNameText;
        public Button selectButton;

        private int themeIndex;

        public void Setup(ThemeSO theme, int index)
        {
            themeIndex = index;
            themeNameText.text = theme.themeName;
            selectButton.onClick.AddListener(SelectTheme);
        }

        void SelectTheme()
        {
            ThemeManager.Instance.SetTheme(themeIndex);
        }
    }
}
