
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PowerUpUpgradePanelUI : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Button openPanelButton;
    [SerializeField] private Button closePanelButton;
    [SerializeField] private Transform powerUpListContainer;
    [SerializeField] private GameObject powerUpCardPrefab;

    public List<PowerUpUpgradeData> powerUpUpgradeDataList; // Assign in inspector

    void Start()
    {
        openPanelButton.onClick.AddListener(OpenPanel);
        closePanelButton.onClick.AddListener(ClosePanel);
        upgradePanel.SetActive(false);
    }

    private void OpenPanel()
    {
        upgradePanel.SetActive(true);
        PopulatePowerUpList();
    }

    private void ClosePanel()
    {
        upgradePanel.SetActive(false);
    }

    private void PopulatePowerUpList()
    {
        foreach (Transform child in powerUpListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var data in powerUpUpgradeDataList)
        {
            GameObject cardGO = Instantiate(powerUpCardPrefab, powerUpListContainer);
            PowerUpUpgradeCardUI cardUI = cardGO.GetComponent<PowerUpUpgradeCardUI>();
            if (cardUI != null)
            {
                cardUI.Setup(data);
            }
        }
    }
}
