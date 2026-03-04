
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "DropTableRegistry", menuName = "Rare Drops/Drop Table Registry")]
public class DropTableRegistry : ScriptableObject
{
    [System.Serializable]
    public class DropItem
    {
        public string itemID; // Maps to an item in a master ItemDatabase
        public RareDropProfileData rarityProfile;
        public int weight;
    }

    [System.Serializable]
    public class DropTable
    {
        public string name;
        public List<DropItem> items = new List<DropItem>();
    }

    public List<DropTable> dropTables = new List<DropTable>();

    public DropItem GetRandomItem(string tableName)
    {
        DropTable table = dropTables.FirstOrDefault(t => t.name == tableName);
        if (table == null || table.items.Count == 0)
        {
            Debug.LogWarning($"Drop table '{tableName}' not found or is empty.");
            return null;
        }

        int totalWeight = table.items.Sum(item => item.weight);
        int randomWeight = UnityEngine.Random.Range(0, totalWeight);

        foreach (var item in table.items)
        {
            if (randomWeight < item.weight)
            {
                return item;
            }
            randomWeight -= item.weight;
        }

        return null; // Should not happen if weights are correct
    }
}
