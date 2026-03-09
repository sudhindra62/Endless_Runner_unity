
using System.Collections.Generic;
using UnityEngine;
using Core;
using Data;
using Managers;

namespace Managers
{
    public class ShopManager : Singleton<ShopManager>
    {
        public List<ShopItem> shopItems;

        public bool PurchaseItem(ShopItem item)
        {
            PlayerData playerData = SaveManager.Instance.GetPlayerData();
            if (playerData.totalCoins >= item.cost)
            {
                playerData.totalCoins -= item.cost;
                SaveManager.Instance.SavePlayerData(playerData);
                ScoreManager.Instance.AddCoin(0); // To update the coin display
                return true;
            }
            else
            {
                UIManager.Instance.ShowPurchaseFailedMessage(2f);
                return false;
            }
        }
    }
}
