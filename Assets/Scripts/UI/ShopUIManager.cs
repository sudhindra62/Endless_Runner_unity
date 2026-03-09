
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Core;
using Managers;

namespace UI
{
    public class ShopUIManager : Singleton<ShopUIManager>
    {
        [Header("Shop UI")]
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private Button openShopButton;
        [SerializeField] private Button closeShopButton;
        [SerializeField] private Transform shopItemContainer;
        [SerializeField] private TextMeshProUGUI purchaseFailedText;

        private Coroutine purchaseFailedCoroutine;

        protected override void Awake()
        {
            base.Awake();
            openShopButton.onClick.AddListener(() => ToggleShopPanel(true));
            closeShopButton.onClick.AddListener(() => ToggleShopPanel(false));
        }

        private void Start()
        {
            shopPanel.SetActive(false);
            purchaseFailedText.gameObject.SetActive(false);
        }

        public void ToggleShopPanel(bool state)
        {
            shopPanel.SetActive(state);
            if (state)
            {
                PopulateShop();
            }
            else
            {
                ClearShop();
            }
        }

        private void PopulateShop()
        {
            foreach (var item in ShopManager.Instance.shopItems)
            {
                GameObject itemGO = ObjectPooler.Instance.SpawnFromPool("ShopItem", shopItemContainer.position, Quaternion.identity);
                itemGO.transform.SetParent(shopItemContainer);
                itemGO.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.itemName;
                itemGO.transform.Find("ItemCost").GetComponent<TextMeshProUGUI>().text = item.cost.ToString();
                itemGO.GetComponent<Button>().onClick.AddListener(() => ShopManager.Instance.PurchaseItem(item));
            }
        }

        private void ClearShop()
        {
            foreach (Transform child in shopItemContainer)
            {
                child.gameObject.SetActive(false);
            }
        }

        public void ShowPurchaseFailedMessage(float duration)
        {
            if (purchaseFailedCoroutine != null)
            {
                StopCoroutine(purchaseFailedCoroutine);
            }
            purchaseFailedCoroutine = StartCoroutine(ShowPurchaseFailedMessageRoutine(duration));
        }

        private System.Collections.IEnumerator ShowPurchaseFailedMessageRoutine(float duration)
        {
            purchaseFailedText.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            purchaseFailedText.gameObject.SetActive(false);
        }
    }
}
