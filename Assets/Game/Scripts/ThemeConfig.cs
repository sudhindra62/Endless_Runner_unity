
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeConfig", menuName = "Theme/Theme Config")]
public class ThemeConfig : ScriptableObject
{
    public string themeName;
    public GameObject themeManagerPrefab;
}
