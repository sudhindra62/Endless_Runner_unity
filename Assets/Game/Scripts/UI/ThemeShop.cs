using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ThemeShop : MonoBehaviour
{
    public GameObject themeItemPrefab;
    public Transform themeItemContainer;

    public List<ThemeConfig> themeConfigs;

    private void Start()
    {
        foreach (ThemeConfig config in themeConfigs)
        {
            GameObject itemGO = Instantiate(themeItemPrefab, themeItemContainer);
            ThemeShopItem item = itemGO.GetComponent<ThemeShopItem>();
            item.Setup(config);
        }
    }
}
