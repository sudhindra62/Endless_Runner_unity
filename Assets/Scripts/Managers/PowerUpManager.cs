
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private CoinDoubler coinDoubler;

    private void Awake()
    {
        // Register this manager with the ServiceLocator
        ServiceLocator.Register<PowerUpManager>(this);
        
        // Find the CoinDoubler component in the scene
        coinDoubler = FindFirstObjectByType<CoinDoubler>();
        if (coinDoubler == null)
        {
            Debug.LogWarning("CoinDoubler component not found in the scene. The coin doubler power-up will not work.");
        }
    }

    private void OnDestroy()
    {
        // Unregister this manager from the ServiceLocator
        ServiceLocator.Unregister<PowerUpManager>();
    }

    public void ActivateCoinDoubler()
    {
        if (coinDoubler != null)
        {
            coinDoubler.Activate();
        }
        else
        {
            Debug.LogWarning("Cannot activate Coin Doubler because the component was not found.");
        }
    }
}
