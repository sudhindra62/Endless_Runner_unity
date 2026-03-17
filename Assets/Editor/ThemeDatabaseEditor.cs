
using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(ThemeDatabase))]
public class ThemeDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ThemeDatabase themeDatabase = (ThemeDatabase)target;

        if (GUILayout.Button("Populate Themes"))
        {
            string[] guids = AssetDatabase.FindAssets("t:ThemeConfig");
            themeDatabase.themes = new ThemeConfig[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                themeDatabase.themes[i] = AssetDatabase.LoadAssetAtPath<ThemeConfig>(path);
            }
            EditorUtility.SetDirty(themeDatabase);
        }
    }
}
