using UnityEngine;

public class BootstrapGameManager : MonoBehaviour
{
    public AdsManager adsManager;
    public IAPManager iapManager;
    public GameServicesManager gameServicesManager;

    private void Start()
    {
        // Intentionally empty
        // Existing project expects this object to exist
    }
}
