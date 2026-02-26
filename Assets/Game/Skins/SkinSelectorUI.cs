using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Manages the main skin selection UI, including a scrollable list of skins.
/// </summary>
public partial class SkinSelectorUI : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private SkinDatabase skinDatabase;

    [Header("UI References")]
    [SerializeField] private Transform skinListContainer;
    [SerializeField] private GameObject skinItemPrefab;
    [SerializeField] private SkinPreviewUI skinPreviewPanel;

    private class SkinListItem
    {
        public SkinData SkinData;
        public GameObject LockIcon;
        public GameObject SelectedIndicator;
    }

    private List<SkinListItem> skinListItems = new List<SkinListItem>();
    private SkinData currentlyDisplayedSkin;

    // -------------------------------------------------
    // ✅ COMPATIBILITY WRAPPER (MOVED HERE — LEGAL)
    // -------------------------------------------------
    private bool SelectSkinSafe(string skinId)
    {
        SkinManager.Instance.SelectSkin(skinId);
        return true;
    }

    private void Start()
    {
        if (skinPreviewPanel == null || skinDatabase == null ||
            skinListContainer == null || skinItemPrefab == null)
        {
            Debug.LogError("SkinSelectorUI: Missing references.");
            gameObject.SetActive(false);
            return;
        }

        skinPreviewPanel.Initialize(this);
        PopulateSkinList();

        if (SkinManager.Instance != null)
        {
            DisplaySkin(
                skinDatabase.GetSkinByID(
                    SkinManager.Instance.GetSelectedSkinID()
                )
            );
        }
    }

    private void OnEnable()
    {
        SkinManager.OnSkinUnlocked += RefreshUI;
        SkinManager.OnSkinSelected += RefreshUI;
        CurrencyManager.OnCoinsChanged += OnCurrencyChanged;
        CurrencyManager.OnGemsChanged += OnCurrencyChanged;
    }

    private void OnDisable()
    {
        SkinManager.OnSkinUnlocked -= RefreshUI;
        SkinManager.OnSkinSelected -= RefreshUI;
        CurrencyManager.OnCoinsChanged -= OnCurrencyChanged;
        CurrencyManager.OnGemsChanged -= OnCurrencyChanged;
    }

    public void OnAttemptUnlock(string skinID)
    {
        SkinManager.Instance?.UnlockSkin(skinID);
    }

    public void OnAttemptSelect(string skinID)
    {
        SkinManager.Instance?.SelectSkin(skinID);
    }

    private void RefreshUI()
    {
        UpdateAllListItemVisuals();
        skinPreviewPanel?.UpdateButtonStates();
    }

    private void PopulateSkinList()
    {
        foreach (Transform child in skinListContainer)
            Destroy(child.gameObject);

        skinListItems.Clear();

        foreach (var skin in skinDatabase.GetAllSkins())
        {
            GameObject item = Instantiate(skinItemPrefab, skinListContainer);
            item.name = $"SkinItem_{skin.SkinID}";

            Button btn = item.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() => DisplaySkin(skin));

            skinListItems.Add(new SkinListItem
            {
                SkinData = skin,
                LockIcon = item.transform.Find("LockIcon")?.gameObject,
                SelectedIndicator = item.transform.Find("SelectedCheckmark")?.gameObject
            });
        }

        UpdateAllListItemVisuals();
    }

    private void DisplaySkin(SkinData skin)
    {
        if (skin == null) return;
        currentlyDisplayedSkin = skin;
        skinPreviewPanel.DisplaySkin(skin);
    }

    private void UpdateAllListItemVisuals()
    {
        foreach (var item in skinListItems)
        {
            bool unlocked = SkinManager.Instance.IsSkinUnlocked(item.SkinData.SkinID);
            bool selected = SkinManager.Instance.GetSelectedSkinID() == item.SkinData.SkinID;

            item.LockIcon?.SetActive(!unlocked);
            item.SelectedIndicator?.SetActive(selected);
        }
    }

    private void OnCurrencyChanged(int _)
    {
        skinPreviewPanel?.UpdateButtonStates();
    }
}
