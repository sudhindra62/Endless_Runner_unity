using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Updates a UI Image to display the character art for the currently selected skin.
/// It listens to the SkinManager's OnSkinChanged event to stay synchronized.
/// </summary>
[RequireComponent(typeof(Image))]
public class CharacterArtDisplay : MonoBehaviour
{
    private Image characterImage;

    void Awake()
    {
        characterImage = GetComponent<Image>();
    }

    void OnEnable()
    {
        SkinManager.OnSkinChanged += UpdateCharacterArt;
        UpdateCharacterArt(); // Initial update
    }

    void OnDisable()
    {
        SkinManager.OnSkinChanged -= UpdateCharacterArt;
    }

    /// <summary>
    /// Fetches the selected skin's data and updates the image sprite.
    /// </summary>
    private void UpdateCharacterArt()
    {
        if (SkinManager.Instance == null) return;

        string selectedSkinID = SkinManager.Instance.GetSelectedSkinID();
        SkinData skinData = SkinManager.Instance.GetSkinData(selectedSkinID);

        if (skinData != null && skinData.characterArt != null)
        {
            characterImage.sprite = skinData.characterArt;
            characterImage.enabled = true;
        }
        else
        {
            // If there's no art, disable the image to keep the UI clean.
            characterImage.enabled = false;
        }
    }

    private void UpdateCharacterArt(string skinId)
    {
        UpdateCharacterArt();
    }
}
