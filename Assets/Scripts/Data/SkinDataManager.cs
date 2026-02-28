
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SkinData
{
    public string skinName;
    public int cost;
    public bool isUnlocked;
}

public class SkinDataManager : MonoBehaviour
{
    public static SkinDataManager Instance { get; private set; }

    public List<SkinData> skins;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockSkin(string skinName)
    {
        SkinData skin = skins.Find(s => s.skinName == skinName);
        if (skin != null && !skin.isUnlocked)
        {
            skin.isUnlocked = true;
            // Deduct cost from player's currency
        }
    }
}
