using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class MagnetUIController : MonoBehaviour
{
    [Header("Active Magnet Display")]
    [SerializeField] private GameObject activeMagnetGroup;
    [SerializeField] private Image magnetTimerFill;
    [SerializeField] private Image activeMagnetIcon;

    [Header("Inventory Display")]
    [SerializeField] private TextMeshProUGUI inventoryCountText;
    [SerializeField] private MagnetTier tierToDisplay;

    private Coroutine activeTimerCoroutine;

    private void OnEnable()
    {
        MagnetUpgradeManager.OnMagnetStateChanged += HandleMagnetStateChanged;
        MagnetInventoryManager.OnMagnetInventoryChanged += HandleInventoryChanged;
    }

    private void OnDisable()
    {
        MagnetUpgradeManager.OnMagnetStateChanged -= HandleMagnetStateChanged;
        MagnetInventoryManager.OnMagnetInventoryChanged -= HandleInventoryChanged;
    }

    private void Start()
    {
        activeMagnetGroup.SetActive(false);
        UpdateInventoryDisplay(tierToDisplay, MagnetInventoryManager.Instance.GetMagnetCount());
    }

    private void HandleMagnetStateChanged(bool isActive, float duration, float radius)
    {
        if (isActive)
        {
            activeMagnetGroup.SetActive(true);
            MagnetTier activeTier = GetTierFromRadius(radius);
            activeMagnetIcon.sprite =
                MagnetUpgradeManager.Instance.GetPropertiesForTier(activeTier).Icon;

            if (activeTimerCoroutine != null)
                StopCoroutine(activeTimerCoroutine);

            activeTimerCoroutine = StartCoroutine(UpdateTimerUI(duration));
        }
        else
        {
            activeMagnetGroup.SetActive(false);
            if (activeTimerCoroutine != null)
                StopCoroutine(activeTimerCoroutine);
        }
    }

    // 🔹 ADDITIVE OVERLOAD (delegate compatibility)
    private void HandleInventoryChanged()
    {
        UpdateInventoryDisplay(tierToDisplay, MagnetInventoryManager.Instance.GetMagnetCount());
    }

    private void HandleInventoryChanged(MagnetTier tier, int newCount)
    {
        if (tier == tierToDisplay)
        {
            UpdateInventoryDisplay(tier, newCount);
        }
    }

    private void UpdateInventoryDisplay(MagnetTier tier, int count)
    {
        inventoryCountText.text = count.ToString();
    }

    private IEnumerator UpdateTimerUI(float totalDuration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < totalDuration)
        {
            magnetTimerFill.fillAmount = 1f - (elapsedTime / totalDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        magnetTimerFill.fillAmount = 0;
    }

    private MagnetTier GetTierFromRadius(float radius)
    {
        foreach (MagnetTier tier in Enum.GetValues(typeof(MagnetTier)))
        {
            if (Mathf.Approximately(
                MagnetUpgradeManager.Instance.GetPropertiesForTier(tier).Radius,
                radius))
            {
                return tier;
            }
        }
        return MagnetTier.Small;
    }
}
