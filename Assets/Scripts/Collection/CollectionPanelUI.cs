
using UnityEngine;
using System.Collections.Generic;

public class CollectionPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform characterTabContent;
    [SerializeField] private Transform cosmeticTabContent;
    [SerializeField] private Transform artifactTabContent;
    [SerializeField] private Transform eventTabContent;

    private void Start()
    {
        PopulateCollection();
    }

    private void PopulateCollection()
    {
        List<CollectionItemData> allItems = CollectionManager.Instance.allCollectionItems;

        foreach (var item in allItems)
        {
            Transform parentTab = GetParentTab(item.itemType);
            GameObject cardGO = Instantiate(cardPrefab, parentTab);
            cardGO.GetComponent<CollectionItemCardUI>().SetItemData(item);
        }
    }

    private Transform GetParentTab(CollectionItemType itemType)
    {
        switch (itemType)
        {
            case CollectionItemType.CharacterFragment:
                return characterTabContent;
            case CollectionItemType.CosmeticFragment:
                return cosmeticTabContent;
            case CollectionItemType.SpecialArtifact:
                return artifactTabContent;
            case CollectionItemType.EventCollectible:
                return eventTabContent;
            default:
                return characterTabContent;
        }
    }
}
