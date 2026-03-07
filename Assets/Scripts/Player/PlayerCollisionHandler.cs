
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerCollisionHandler : MonoBehaviour
{
    [Header("Collision Layers")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask coinLayer;
    [SerializeField] private LayerMask powerUpLayer;

    private PlayerController playerController;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing) return;

        if ((obstacleLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            HandleObstacleCollision();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.GetCurrentState() != GameManager.GameState.Playing) return;

        if ((coinLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            HandleCoinCollision(other.gameObject);
        }
        else if ((powerUpLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            HandlePowerUpCollision(other.gameObject);
        }
    }

    private void HandleObstacleCollision()
    {
        // if (PowerUpManager.Instance != null && PowerUpManager.Instance.IsShieldActive())
        // {
        //     PowerUpManager.Instance.DeactivateShield();
        //     return; // Shield protected the player
        // }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RevivePlayer(); 
        }
    }

    private void HandleCoinCollision(GameObject coinObject)
    {
        // if (ScoreManager.Instance != null)
        // {
        //     ScoreManager.Instance.AddCoin();
        // }
        coinObject.SetActive(false); // Should be returned to an object pool
    }

    private void HandlePowerUpCollision(GameObject powerUpObject)
    {
        // PowerUpCollectible powerUp = powerUpObject.GetComponent<PowerUpCollectible>();
        // if (PowerUpManager.Instance != null && powerUp != null)
        // {
        //     PowerUpManager.Instance.CollectPowerUp(powerUp.powerUpType);
        // }
        powerUpObject.SetActive(false); // Should be returned to an object pool
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        // Disable collision handling when not in playing state
        if (newState != GameManager.GameState.Playing)
        {
            // You might want to temporarily disable the collider to prevent unwanted physics interactions
        }
    }
}
