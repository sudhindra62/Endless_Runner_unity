
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Reflection;

[InitializeOnLoad]
public class InputActionsGenerator
{
    static InputActionsGenerator()
    {
        GenerateCSharpClass();
    }

    public static void GenerateCSharpClass()
    {
        var inputActionAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>("Assets/Settings/PlayerInput.inputactions");
        if (inputActionAsset == null)
        {
            return;
        }

        var scriptPath = "Assets/Scripts/Managers/PlayerInput.cs";
        MethodInfo generatorMethod = typeof(InputActionAsset).GetMethod(
            "GenerateCSharpCode",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (generatorMethod == null)
        {
            Debug.LogWarning("Input action wrapper generation is unavailable in this Input System version.");
            return;
        }

        string scriptContent = generatorMethod.Invoke(
            inputActionAsset,
            new object[] { "PlayerInput", "Managers", null, null, null }) as string;

        if (string.IsNullOrEmpty(scriptContent))
        {
            Debug.LogWarning("Input action wrapper generation returned no content.");
            return;
        }

        if (!System.IO.File.Exists(scriptPath) || System.IO.File.ReadAllText(scriptPath) != scriptContent)
        {
            System.IO.File.WriteAllText(scriptPath, scriptContent);
            AssetDatabase.Refresh();
        }
    }
}
