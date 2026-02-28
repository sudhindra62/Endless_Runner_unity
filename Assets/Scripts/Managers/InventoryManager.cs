
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
        // In a real implementation, this would check if the player has a revive token.
        // For now, we'll just return true.
        return true;
    }

    public void UseReviveToken()
    {
        // In a real implementation, this would consume a revive token.
    }
}
