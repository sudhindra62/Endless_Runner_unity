
using UnityEngine;

public class SkinSelectorUI : MonoBehaviour
{
    public SkinDatabase skinDatabase;
    public SkinPreviewUI skinPreview;
    private int _currentSkinIndex = 0;

    public void NextSkin()
    {
        _currentSkinIndex = (_currentSkinIndex + 1) % skinDatabase.skinCatalog.skins.Length;
        skinPreview.UpdatePreview(skinDatabase.skinCatalog.skins[_currentSkinIndex]);
    }

    public void PreviousSkin()
    {
        _currentSkinIndex--;
        if (_currentSkinIndex < 0)
        {
            _currentSkinIndex = skinDatabase.skinCatalog.skins.Length - 1;
        }
        skinPreview.UpdatePreview(skinDatabase.skinCatalog.skins[_currentSkinIndex]);
    }
}
