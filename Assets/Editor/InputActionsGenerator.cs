
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

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
        var scriptContent = inputActionAsset.GenerateCSharpCode(className: "PlayerInput", namespaceName: "Managers");

        if (!System.IO.File.Exists(scriptPath) || System.IO.File.ReadAllText(scriptPath) != scriptContent)
        {
            System.IO.File.WriteAllText(scriptPath, scriptContent);
            AssetDatabase.Refresh();
        }
    }
}
