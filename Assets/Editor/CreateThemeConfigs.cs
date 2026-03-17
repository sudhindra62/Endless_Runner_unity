using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateThemeConfigs : EditorWindow
{
    [MenuItem("Tools/Create Theme Configs")]
    public static void CreateConfigs()
    {
        string themesFolderPath = "Assets/Game/Themes";
        string[] themeFolders = Directory.GetDirectories(themesFolderPath);

        foreach (string themeFolderPath in themeFolders)
        {
            string assetPath = Path.Combine(themeFolderPath, "ThemeConfig.asset");

            if (!File.Exists(assetPath))
            {
                ThemeConfig newConfig = ScriptableObject.CreateInstance<ThemeConfig>();
                AssetDatabase.CreateAsset(newConfig, assetPath);
                Debug.Log($"Created ThemeConfig asset for {Path.GetFileName(themeFolderPath)} at: {assetPath}");
            }
            else
            {
                Debug.Log($"ThemeConfig asset already exists for {Path.GetFileName(themeFolderPath)}.");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
