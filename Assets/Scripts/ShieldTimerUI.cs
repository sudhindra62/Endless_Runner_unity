using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the shield power-up timer UI, integrated with the GameHUDController.
/// </summary>
public class ShieldTimerUI : MonoBehaviour
{
    public static ShieldTimerUI Instance { get; private set; }

    // --- Cached References from GameHUDController ---
    private Image shieldFillImage;
    private GameObject shieldContainer;

    private float duration;
    private float timeLeft;
    private bool isTimerRunning;

    private void Awake()
    {
        // Singleton pattern
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

    private void Start()
    {
        // Get UI element references from the central GameHUDController
        if (GameHUDController.Instance != null)
        {
            shieldFillImage = GameHUDController.Instance.ShieldFillImage;
            shieldContainer = GameHUDController.Instance.ShieldContainer;
        }
        else
        {
            Debug.LogError("ShieldTimerUI requires an instance of GameHUDController.");
        }
    }

    private void Update()
    {
        // Only run the timer logic when it's active
        if (!isTimerRunning) return;

        timeLeft -= Time.unscaledDeltaTime; // Use unscaled time to run even when paused

        if (shieldFillImage != null)
        {
            shieldFillImage.fillAmount = timeLeft / duration;
        }

        if (timeLeft <= 0)
        {
            StopTimer();
        }
    }

    /// <summary>
    /// Starts the shield timer and displays the UI.
    /// </summary>
    public void StartTimer(float seconds)
    {
        if (shieldContainer == null) return;

        duration = seconds;
        timeLeft = seconds;
        isTimerRunning = true;
        
        shieldContainer.SetActive(true);
    }

    /// <summary>
    /// Stops the shield timer and hides the UI.
    /// </summary>
    public void StopTimer()
    {
        isTimerRunning = false;

        if (shieldContainer != null)
        {
            shieldContainer.SetActive(false);
        }
    }
}
