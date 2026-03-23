using UnityEngine;
using UnityEngine.UI;


public class SkinButtonUI : MonoBehaviour
{
    public Button button;
    public Image skinImage;

    private SkinData _skinData;
    private SkinPurchaseManager _skinPurchaseManager;
    private SkinPreviewUI _skinPreviewUI;

    public void Setup(SkinData skinData)
    {
        _skinData = skinData;
        skinImage.sprite = _skinData.sprite;
        _skinPurchaseManager = ServiceLocator.Get<SkinPurchaseManager>();
        _skinPreviewUI = FindFirstObjectByType<SkinPreviewUI>(); // A better solution would be a ServiceLocator or direct reference

        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        _skinPreviewUI.ShowSkin(_skinData);
        // Additional logic for purchase button can be added here, 
        // for example, showing a confirmation popup.
        _skinPurchaseManager.PurchaseSkin(_skinData);
    }
}
