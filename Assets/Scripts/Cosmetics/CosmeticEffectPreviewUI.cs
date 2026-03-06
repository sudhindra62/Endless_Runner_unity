
using UnityEngine;
using UnityEngine.UI;

public class CosmeticEffectPreviewUI : MonoBehaviour
{
    [SerializeField] private Image previewImage;
    [SerializeField] private RawImage animatedPreview;
    [SerializeField] private Text rarityText;
    [SerializeField] private Text unlockMethodText;
    [SerializeField] private Button equipButton;

    private CosmeticEffectData currentEffect;

    public void DisplayEffect(CosmeticEffectData effectData)
    {
        currentEffect = effectData;

        // Show animated or static preview based on rarity
        bool isAnimated = effectData.rarity == CosmeticRarity.Legendary || effectData.rarity == CosmeticRarity.Mythic;
        if (animatedPreview != null) animatedPreview.gameObject.SetActive(isAnimated);
        if (previewImage != null) previewImage.gameObject.SetActive(!isAnimated);

        rarityText.text = effectData.rarity.ToString();
        unlockMethodText.text = $"Unlock via: {effectData.unlockMethod}";

        bool isUnlocked = CosmeticInventoryManager.Instance.IsCosmeticUnlocked(effectData.effectID);
        equipButton.interactable = isUnlocked;
        equipButton.GetComponentInChildren<Text>().text = isUnlocked ? "Equip" : "Locked";

        equipButton.onClick.RemoveAllListeners();
        if (isUnlocked)
        {
            equipButton.onClick.AddListener(OnEquipClicked);
        }
    }

    private void OnEquipClicked()
    {
        CosmeticInventoryManager.Instance.EquipCosmetic(currentEffect.effectID, currentEffect.effectType);
    }
}
