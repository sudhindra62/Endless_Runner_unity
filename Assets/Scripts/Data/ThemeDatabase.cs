using UnityEngine;


[CreateAssetMenu(fileName = "ThemeDatabase", menuName = "Theme/Theme Database")]
public class ThemeDatabase : ScriptableObject
{
    public ThemeSO[] themes;
    public ThemeSO[] themeSOs;
}

