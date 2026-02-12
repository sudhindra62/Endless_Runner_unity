using UnityEngine;
using System.Collections;

/// <summary>
/// A consolidated manager for the Shield power-up. It handles inventory, activation logic, 
/// and direct control over the player's shield visual effect and UI timer. This approach 
/// improves performance and simplifies the architecture by removing event-based communication.
/// </summary>
public class ShieldPowerupManager : MonoBehaviour
{
    public static ShieldPowerupManager Instance { get; private set; }

    [Header("Shield Settings")]
    [SerializeField] private float shieldDuration = 10f; // Default duration
    [SerializeField] private GameObject playerShieldVisual; // Assign the player's shield GameObject

    private int shieldCount;
    private bool isShieldActive = false;

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
            InitializeShield();
        }
    }

    private void InitializeShield()
    {
        shieldCount = 0; // Load from save data in a real game
        if (playerShieldVisual != null)
        {
            playerShieldVisual.SetActive(false);
        }
    }

    /// <summary>
    /// Uses a shield from inventory if available.
    /// </summary>
    public void UseShield()
    {
        if (shieldCount > 0 && !isShieldActive)
        {
            shieldCount--;
            StartCoroutine(ShieldCoroutine());
            // Update any relevant UI immediately
        }
        else
        {
            Debug.LogWarning("Cannot use shield. No shields in inventory or shield already active.");
        }
    }

    /// <summary>
    /// Adds shields to the inventory.
    /// </summary>
    public void AddShields(int quantity)
    {
        shieldCount += quantity;
        // Update UI
    }

    public int GetShieldCount()
    {
        return shieldCount;
    }

    public bool IsShieldActive()
    {
        return isShieldActive;
    }

    private IEnumerator ShieldCoroutine()
    {
        isShieldActive = true;
        ActivateShieldVisual(true);

        float timer = shieldDuration;
        while (timer > 0)
        {
            if (!GameStateManager.Instance.IsPlaying())
            {
                yield return null; // Pause the timer if the game is not in play
                continue;
            }
            timer -= Time.deltaTime;
            UpdateShieldUI(timer / shieldDuration);
            yield return null;
        }

        DeactivateShield();
    }

    /// <summary>
    /// Deactivates the shield prematurely, e.g., after a collision.
    /// </summary>
    public void DeactivateShield()
    {
        if (isShieldActive)
        {
            isShieldActive = false;
            ActivateShieldVisual(false);
            UpdateShieldUI(0);
            StopAllCoroutines(); // Stop the active shield coroutine
        }
    }

    private void ActivateShieldVisual(bool isActive)
    {
        if (playerShieldVisual != null)
        {
            playerShieldVisual.SetActive(isActive);
        }
    }

    private void UpdateShieldUI(float fillAmount)
    {
        if (GameHUDController.Instance != null)
        {
            GameHUDController.Instance.ShieldContainer.SetActive(fillAmount > 0);
            GameHUDController.Instance.ShieldFillImage.fillAmount = fillAmount;
        }
    }
}
