using UnityEngine;
using UnityEngine.InputSystem;

    public static class Input
    {
        public static bool GetButtonDown(string buttonName)
        {
            // Simple mapping for common buttons
            if (buttonName == "Jump") return Keyboard.current?.spaceKey.wasPressedThisFrame ?? false;
            return false;
        }

        public static bool GetKeyDown(KeyCode key)
        {
            return UnityEngine.Input.GetKeyDown(key);
        }
    }

