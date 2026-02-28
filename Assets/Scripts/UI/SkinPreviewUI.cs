using UnityEngine;
using UnityEngine.UI;
using Skins;

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
}
