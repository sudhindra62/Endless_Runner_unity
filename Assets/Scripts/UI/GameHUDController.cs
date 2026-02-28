
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A clean, simple Singleton that holds references to the in-game HUD UI elements.
/// This script does NOT create UI elements. Instead, you must drag and drop the corresponding
/// UI elements from your scene onto the public fields of this script in the Unity Inspector.
/// This provides a stable, centralized point of access for all other gameplay scripts.
/// </summary>
public class GameHUDController : Singleton<GameHUDController>
{
    // --- Assign these in the Unity Inspector ---
    [Header("Scene UI Element References")]
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TimerText;
    public TextMeshProUGUI MultiplierText;
    public Animator MultiplierAnimator;
    public Image ShieldFillImage;
    public GameObject ShieldContainer;
    public Button PauseButton;

    protected override void Awake()
    {
        base.Awake();
        // --- Initial Validation ---
        // It's good practice to check if the essential references have been assigned.
        if (ScoreText == null || MultiplierText == null || PauseButton == null)
        {
            Debug.LogError("Critical HUD elements are not assigned in the GameHUDController Inspector! Please assign them.");
        }
    }
}
