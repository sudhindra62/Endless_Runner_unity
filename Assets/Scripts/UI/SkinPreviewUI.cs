using UnityEngine;
using UnityEngine.UI;


public class SkinPreviewUI : MonoBehaviour
{
    public Image skinPreviewImage;
    public Text skinNameText;
    public Text skinRarityText;

    public void ShowSkin(SkinData skin)
    {
        skinPreviewImage.sprite = skin.sprite;
        skinNameText.text = skin.skinName;
        skinRarityText.text = skin.rarity.ToString();
    }

    public void UpdatePreview(SkinData skin)
    {
        if (skin != null)
        {
            ShowSkin(skin);
        }
    }
}
