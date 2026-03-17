
using System.Collections.Generic;
using UnityEngine;

namespace EndlessRunner.Character
{
    public class CharacterSkinShopUI : MonoBehaviour
    {
        public GameObject skinItemPrefab;
        public Transform itemContainer;

        private void Start()
        {
            PopulateShop();
        }

        private void PopulateShop()
        {
            foreach (Transform child in itemContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var skin in CharacterSkinManager.Instance.allSkins)
            {
                GameObject itemGO = Instantiate(skinItemPrefab, itemContainer);
                // Assuming the prefab has a script to set up the skin details
                // var itemUI = itemGO.GetComponent<CharacterSkinItemUI>(); 
                // itemUI.Setup(skin);
            }
        }
    }
}
