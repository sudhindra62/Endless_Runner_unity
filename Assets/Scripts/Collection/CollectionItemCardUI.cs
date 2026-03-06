
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectionItemCardUI : MonoBehaviour
{
    public CollectionItemData itemData;

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI fragmentCountText;
    [SerializeField] private Slider progressBar;
    [SerializeField] private Button unlockButton;
    [SerializeField] private Button purchaseFragmentsButton; // Monetization option

    private void Start()
    {
        if (purchaseFragmentsButton != null)
        {
            purchaseFragmentsButton.onClick.AddListener(OnPurchaseFragments);
        }
    }

    public void SetItemData(CollectionItemData data)
    {
        this.itemData = data;
        UpdateUI();
        CollectionInventoryManager.OnInventoryChanged += OnInventoryUpdated;
    }

    private void OnInventoryUpdated(CollectionItemData updatedItemData)
    {
        if (updatedItemData == this.itemData)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (itemData == null) return;

        itemIcon.sprite = itemData.itemIcon;
        int currentFragments = CollectionInventoryManager.Instance.GetFragmentCount(itemData);
        fragmentCountText.text = currentFragments + " / " + itemData.requiredFragments;
        progressBar.maxValue = itemData.requiredFragments;
        progressBar.value = currentFragments;

        bool isUnlocked = currentFragments >= itemData.requiredFragments;
        if (unlockButton != null)
        {
            unlockButton.gameObject.SetActive(isUnlocked);
            unlockButton.interactable = isUnlocked;
        }
    }

    private void OnPurchaseFragments()
    {
        // In a real game, this would trigger an in-app purchase flow
        Debug.Log("Purchase fragments for " + itemData.itemName + " clicked");
        CollectionInventoryManager.Instance.AddFragments(itemData, 5); // Example: Adds 5 fragments
    }

    private void OnDestroy()
    {
        CollectionInventoryManager.OnInventoryChanged -= OnInventoryUpdated;
    }
}
