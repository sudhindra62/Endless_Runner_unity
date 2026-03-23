
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
            string[] guids = AssetDatabase.FindAssets("t:ThemeSO");
            themeDatabase.themes = new ThemeSO[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                themeDatabase.themes[i] = AssetDatabase.LoadAssetAtPath<ThemeSO>(path);
            }
            EditorUtility.SetDirty(themeDatabase);
        }
    }
}
