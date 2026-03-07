
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance { get; private set; }

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

    public void ActivatePowerUp(PowerUp powerUp)
    {
        // In a real game, you would find the player and activate the power-up.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            powerUp.Activate(player);
            // You might want to start a coroutine to deactivate the power-up after its duration.
        }
    }
}
