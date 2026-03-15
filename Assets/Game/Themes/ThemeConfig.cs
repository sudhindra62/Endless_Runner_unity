
using UnityEngine;

namespace EndlessRunner.Themes
{
    [CreateAssetMenu(fileName = "ThemeConfig", menuName = "EndlessRunner/Theme Config", order = 2)]
    public class ThemeConfig : ScriptableObject
    {
        public ThemeSO theme;
    }
}
