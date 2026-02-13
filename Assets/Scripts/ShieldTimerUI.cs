using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Unified ShieldTimerUI
/// Preserves:
/// - Singleton pattern
/// - DontDestroyOnLoad
/// - Inspector-based Image reference
/// - GameHUDController integration
/// - Shield container support
/// - Unscaled time handling
/// </summary>
public class ShieldTimerUI : MonoBehaviour
{
    public static ShieldTimerUI Instance { get; private set; }

    [Header("Optional Direct Reference (Fallback Mode)")]
    public Image fillImage; // Optional inspector assignment

    private Image shieldFillImage;     // From GameHUDController
    private GameObject shieldContainer; // From GameHUDController

    private float duration;
    private float timeLeft;
    private bool isTimerRunning;

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

    private void Start()
    {
        // Try to bind from GameHUDController
        if (GameHUDController.Instance != null)
        {
            shieldFillImage = GameHUDController.Instance.ShieldFillImage;
            shieldContainer = GameHUDController.Instance.ShieldContainer;
        }

        // Fallback: use local fillImage if provided
        if (shieldFillImage == null && fillImage != null)
        {
            shieldFillImage = fillImage;
        }
    }

    private void Update()
    {
        if (!isTimerRunning) return;

        timeLeft -= Time.unscaledDeltaTime;

        if (shieldFillImage != null && duration > 0f)
        {
            shieldFillImage.fillAmount = Mathf.Clamp01(timeLeft / duration);
        }

        if (timeLeft <= 0f)
        {
            StopTimer();
        }
    }

    /// <summary>
    /// Starts the shield timer and displays the UI.
    /// </summary>
    public void StartTimer(float seconds)
    {
        duration = seconds;
        timeLeft = seconds;
        isTimerRunning = true;

        if (shieldContainer != null)
            shieldContainer.SetActive(true);
        else
            gameObject.SetActive(true);
    }

    /// <summary>
    /// Stops the shield timer and hides the UI.
    /// </summary>
    public void StopTimer()
    {
        isTimerRunning = false;

        if (shieldContainer != null)
            shieldContainer.SetActive(false);
        else
            gameObject.SetActive(false);
    }
}
