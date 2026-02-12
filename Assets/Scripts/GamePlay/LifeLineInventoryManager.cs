using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Defines the different types of consumable lifelines available to the player.
/// </summary>
public enum LifeLineType
{
    Shield,
    ReviveOnce,
    SlowMotionEscape,
    AutoJumpRescue
}

/// <summary>
/// Manages the player's persistent inventory of all lifeline items.
/// This acts as a central data store for other gameplay systems.
/// </summary>
public class LifeLineInventoryManager : MonoBehaviour
{
    public static LifeLineInventoryManager Instance { get; private set; }

    private Dictionary<LifeLineType, int> inventory = new Dictionary<LifeLineType, int>();

    // Event signature: LifeLineType, new count
    public static event Action<LifeLineType, int> OnInventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInventory();
        }
    }

    private void InitializeInventory()
    {
        // Load from save data in a real game. For now, initialize all to 0.
        foreach (LifeLineType type in Enum.GetValues(typeof(LifeLineType)))
        {
            if (!inventory.ContainsKey(type))
            {
                inventory.Add(type, 0);
            }
        }
    }

    /// <summary>
    /// Adds a specified quantity of a lifeline item to the inventory.
    /// </summary>
    public void AddLifeLines(LifeLineType type, int amount)
    {
        if (inventory.ContainsKey(type))
        {
            inventory[type] += amount;
            OnInventoryChanged?.Invoke(type, inventory[type]);
            Debug.Log($"Added {amount} of {type}. New total: {inventory[type]}.");
        }
    }

    /// <summary>
    /// Consumes one lifeline item of the specified type from the inventory.
    /// </summary>
    /// <returns>True if the item was available and used, false otherwise.</returns>
    public bool UseLifeLine(LifeLineType type)
    {
        if (GetItemCount(type) > 0)
        {
            inventory[type]--;
            OnInventoryChanged?.Invoke(type, inventory[type]);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Gets the current count of a specific lifeline item.
    /// </summary>
    public int GetItemCount(LifeLineType type)
    {
        return inventory.ContainsKey(type) ? inventory[type] : 0;
    }
}
