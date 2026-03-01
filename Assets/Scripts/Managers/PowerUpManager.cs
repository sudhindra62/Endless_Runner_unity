
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

    private bool arePowerUpsDisabled = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPowerUpsDisabled(bool isDisabled)
    {
        arePowerUpsDisabled = isDisabled;
        Debug.Log(isDisabled ? "Power-ups are now DISABLED." : "Power-ups are now ENABLED.");
    }

    public bool ArePowerUpsDisabled()
    {
        return arePowerUpsDisabled;
    }

    // Example method that would be called before spawning a power-up
    public bool CanSpawnPowerUp()
    {
        if (arePowerUpsDisabled)
        {
            Debug.Log("Power-up spawn blocked by NoPowerUpsChallenge.");
            return false;
        }
        // Other logic for spawning power-ups would go here
        return true;
    }
}
