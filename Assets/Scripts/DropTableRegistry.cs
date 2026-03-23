
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

    /// <summary>
    /// A ScriptableObject that holds all drop tables for the game.
    /// Refactored by the Supreme Guardian Architect v13 to use a dictionary for high-performance lookups.
    /// </summary>
    [CreateAssetMenu(fileName = "DropTableRegistry", menuName = "Rare Drops/Drop Table Registry")]
    public class DropTableRegistry : ScriptableObject
    {
        [System.Serializable]
        public class DropItem
        {
            [Tooltip("Maps to a unique item ID in a master ItemDatabase.")]
            public string itemID;
            [Tooltip("The rarity profile governing this item's drop chance.")]
            public RareDropProfileData rarityProfile;
            [Tooltip("The relative chance for this item to be chosen from the table.")]
            public int weight;
        }

        [System.Serializable]
        public class DropTable
        {
            [Tooltip("Unique name for this drop table (e.g., 'CommonChest', 'BossDragon').")]
            public string name;
            public List<DropItem> items = new List<DropItem>();
        }

        [Header("Designer-Friendly Configuration")]
        [Tooltip("Add and configure all game drop tables here.")]
        public List<DropTable> dropTables = new List<DropTable>();

        private Dictionary<string, DropTable> _tableLookup;

        private void OnEnable()
        {
            InitializeLookup();
        }

        private void InitializeLookup()
        {
            _tableLookup = new Dictionary<string, DropTable>();
            foreach (var table in dropTables)
            {
                if (table != null && !string.IsNullOrEmpty(table.name) && !_tableLookup.ContainsKey(table.name))
                {
                    _tableLookup.Add(table.name, table);
                }
                else
                {
                    Debug.LogWarning($"Guardian Architect Warning: A drop table in '{this.name}' has a duplicate or invalid name and was ignored: '{table?.name}'");
                }
            }
            Debug.Log($"Guardian Architect: DropTableRegistry '{this.name}' initialized with {_tableLookup.Count} tables.");
        }

        public DropItem GetRandomItem(string tableName)
        {
            #if UNITY_EDITOR
            if (_tableLookup == null || _tableLookup.Count != dropTables.Count)
            {
                InitializeLookup();
            }
            #endif

            if (_tableLookup == null)
            {
                Debug.LogError("Guardian Architect FATAL_ERROR: Drop table lookup is not initialized!");
                return null;
            }

            if (!_tableLookup.TryGetValue(tableName, out DropTable table) || table.items.Count == 0)
            {
                Debug.LogWarning($"Guardian Architect: Drop table '{tableName}' not found or is empty in '{this.name}'.");
                return null;
            }

            int totalWeight = table.items.Sum(item => item.weight);
            if (totalWeight <= 0)
            { 
                Debug.LogWarning($"Guardian Architect: Drop table '{tableName}' has a total weight of zero. Cannot drop an item.");
                return null;
            }

            int randomWeight = UnityEngine.Random.Range(0, totalWeight);

            foreach (var item in table.items)
            {
                if (randomWeight < item.weight)
                {
                    return item;
                }
                randomWeight -= item.weight;
            }

            return null;
        }
    }

