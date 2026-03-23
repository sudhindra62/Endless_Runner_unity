using UnityEngine;
public class SkinsManager : MonoBehaviour
{
    public static SkinsManager Instance;
    private void Awake() { Instance = this; }

    public bool IsSkinUnlocked(string skinId)
    {
        if (SkinManager.Instance != null)
        {
            return SkinManager.Instance.IsSkinUnlocked(skinId);
        }

        return SaveManager.Instance != null && SaveManager.Instance.Data.unlockedSkins.Contains(skinId);
    }

    public void UnlockSkin(string skinId)
    {
        if (SkinManager.Instance != null)
        {
            SkinManager.Instance.UnlockSkin(skinId);
            return;
        }

        if (SaveManager.Instance == null) return;
        if (!SaveManager.Instance.Data.unlockedSkins.Contains(skinId))
        {
            SaveManager.Instance.Data.unlockedSkins.Add(skinId);
            SaveManager.Instance.SaveGame();
        }
    }
}
