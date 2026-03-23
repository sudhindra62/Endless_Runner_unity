
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<InventoryManager>();
    }

    public bool HasReviveToken()
    {
        return PlayerDataManager.Instance.HasItem("ReviveToken");
    }

    public void UseReviveToken()
    {
        PlayerDataManager.Instance.RemoveItem("ReviveToken");
    }

    // --- Type Conversion Bridges (Phase 2A: Type Consistency) ---
    
    public bool HasItem(string itemId)
    {
        return PlayerDataManager.Instance.HasItem(itemId);
    }

    public void AddItem(string itemId)
    {
        PlayerDataManager.Instance.AddItem(itemId);
    }

    public void RemoveItem(string itemId)
    {
        PlayerDataManager.Instance.RemoveItem(itemId);
    }

    public System.Collections.Generic.List<string> GetInventory()
    {
        return PlayerDataManager.Instance.GetInventory();
    }

    public int GetInventoryCount()
    {
        var inventory = GetInventory();
        return inventory != null ? inventory.Count : 0;
    }
}
