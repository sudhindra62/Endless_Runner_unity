using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A clean, simple Singleton that holds references to the in-game HUD UI elements.
/// This script does NOT create UI elements. Instead, you must drag and drop the corresponding
/// UI elements from your scene onto the public fields of this script in the Unity Inspector.
/// This provides a stable, centralized point of access for all other gameplay scripts.
/// </summary>
public class GameHUDController : MonoBehaviour
{
    public static GameHUDController Instance { get; private set; }

    // --- Assign these in the Unity Inspector ---
    [Header("Scene UI Element References")]
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI MultiplierText;
    public Animator MultiplierAnimator;
    public Image ShieldFillImage;
    public GameObject ShieldContainer;
    public Button PauseButton;

    private void Awake()
    {
        // Standard Singleton pattern: Ensure only one instance exists.
        if (Instance == null)
        {
            Instance = this;
            // We don't use DontDestroyOnLoad here, as the HUD is specific to the gameplay scene.
            // If you have a single, persistent scene, you can add DontDestroyOnLoad(gameObject).
        }
        else
        {
            Debug.LogWarning("Duplicate GameHUDController detected. Destroying the new one.");
            Destroy(gameObject);
        }

        // --- Initial Validation ---
        // It's good practice to check if the essential references have been assigned.
        if (ScoreText == null || MultiplierText == null || PauseButton == null)
        {
            Debug.LogError("Critical HUD elements are not assigned in the GameHUDController Inspector! Please assign them.");
        }
    }
}
