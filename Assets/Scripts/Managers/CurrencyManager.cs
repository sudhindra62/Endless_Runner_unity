
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<CurrencyManager>();
    }

    public bool SpendGems(int amount)
    {
        // In a real implementation, this would check if the player has enough gems
        // and then subtract them. For now, we'll just return true.
        return true;
    }
}
