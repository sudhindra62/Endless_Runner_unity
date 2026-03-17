using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ThemeManager))]
public class ThemeSwapper : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ThemeManager themeManager = (ThemeManager)target;

        EditorGUILayout.Space();

        themeManager.currentTheme = (ThemeManager.Theme)EditorGUILayout.EnumPopup("Current Theme", themeManager.currentTheme);

        if (GUILayout.Button("Set Theme"))
        {
            themeManager.SetTheme(themeManager.currentTheme);
        }
    }
}
