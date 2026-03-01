
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RareDropUI : MonoBehaviour
{
    [SerializeField] private GameObject rareDropPopupPrefab; // Assign a prefab with Text, Image, etc.
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private AudioClip rareDropSound;
    [SerializeField] private Color flashColor = Color.yellow;

    private void Start()
    {
        RareDropManager.OnRareDrop += HandleRareDrop;
    }

    private void OnDestroy()
    {
        RareDropManager.OnRareDrop -= HandleRareDrop;
    }

    private void HandleRareDrop(RareDropType type, int amount)
    {
        StartCoroutine(ShowRareDropPopup(type, amount));
    }

    private IEnumerator ShowRareDropPopup(RareDropType type, int amount)
    {
        // 1. Instantiate Popup
        GameObject popupInstance = Instantiate(rareDropPopupPrefab, transform); // Make it a child of the canvas

        // 2. Set Text and Rarity Color
        Text popupText = popupInstance.GetComponentInChildren<Text>();
        Image rarityIndicator = popupInstance.transform.Find("RarityIndicator").GetComponent<Image>(); // Assuming a child object
        if (popupText != null)
        {
            popupText.text = $"RARE DROP!\n{type} +{amount}";
        }
        if (rarityIndicator != null)
        { 
            rarityIndicator.color = GetRarityColor(type);
        }

        // 3. Play Sound and Screen Flash
        if (rareDropSound != null) AudioSource.PlayClipAtPoint(rareDropSound, Camera.main.transform.position);
        StartCoroutine(ScreenFlash());

        // 4. Animate In (e.g., scale up, fade in)
        // This part would be handled by an Animator component on the prefab

        // 5. Wait for duration
        yield return new WaitForSeconds(displayDuration);

        // 6. Animate Out and Destroy
        Destroy(popupInstance);
    }

    private Color GetRarityColor(RareDropType type)
    {
        switch (type)
        {
            case RareDropType.GoldenBurst:
            case RareDropType.XPBoost:
                return Color.green; // Common
            case RareDropType.GemFragment:
            case RareDropType.LeaguePointBoost:
                return Color.blue; // Uncommon
            case RareDropType.RareChest:
            case RareDropType.SuperPowerUp:
                return Color.magenta; // Rare
            case RareDropType.CosmeticFragment:
            case RareDropType.MysteryReward:
                return Color.yellow; // Epic
            default:
                return Color.white;
        }
    }

    private IEnumerator ScreenFlash()
    {
        // Create a temporary full-screen UI Image for the flash
        GameObject flashPanel = new GameObject("ScreenFlash");
        flashPanel.transform.SetParent(transform, false);
        Image flashImage = flashPanel.AddComponent<Image>();
        flashImage.rectTransform.anchorMin = Vector2.zero;
        flashImage.rectTransform.anchorMax = Vector2.one;
        flashImage.rectTransform.sizeDelta = Vector2.zero;
        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0.5f);

        yield return new WaitForSeconds(0.1f);
        flashImage.color = Color.clear;
        yield return new WaitForSeconds(0.1f);
        Destroy(flashPanel);
    }
}
