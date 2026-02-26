using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinButtonUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image skinIcon;
    [SerializeField] private GameObject lockOverlay;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private GameObject selectionHighlight;

    private SkinData assignedSkinData;
    private Button uiButton;

    void Awake()
    {
        uiButton = GetComponent<Button>();
        uiButton.onClick.AddListener(OnButtonClicked);
    }

    public void Setup(SkinData skinData)
    {
        assignedSkinData = skinData;
        skinIcon.sprite = skinData.skinIcon;
        UpdateState();
    }

    public void UpdateState()
    {
        if (assignedSkinData == null) return;

        string id = assignedSkinData.skinID;

        bool isUnlocked = SkinManager.Instance.IsSkinUnlocked(id);
        bool isSelected = SkinManager.Instance.GetSelectedSkinID() == id;

        lockOverlay.SetActive(!isUnlocked);
        selectionHighlight.SetActive(isSelected);

        if (isUnlocked)
        {
            costText.text = "";
        }
        else
        {
            switch (assignedSkinData.unlockType)
            {
                case SkinUnlockType.Gems:
                    costText.text = assignedSkinData.cost.ToString();
                    break;

                case SkinUnlockType.Paid:
                    costText.text = "PREMIUM";
                    break;

                default:
                    costText.text = "";
                    break;
            }
        }
    }

    private void OnButtonClicked()
    {
        string id = assignedSkinData.skinID;

        if (SkinManager.Instance.IsSkinUnlocked(id))
        {
            SkinManager.Instance.SelectSkin(id);
        }
        else if (assignedSkinData.unlockType == SkinUnlockType.Gems)
        {
            bool unlocked = SkinManager.Instance.UnlockSkinBool(id);
            if (unlocked)
            {
                SkinManager.Instance.SelectSkin(id);
            }
            else
            {
                Debug.Log("Failed to unlock skin: " + assignedSkinData.displayName);
            }
        }
    }
}
