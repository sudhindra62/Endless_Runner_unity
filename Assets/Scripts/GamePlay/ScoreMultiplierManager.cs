using UnityEngine;
using TMPro;
using System;


/// <summary>
/// Manages the score multiplier.
/// </summary>
public class ScoreMultiplierManager : MonoBehaviour
{
    public static event Action<float> OnMultiplierChanged;

    public static ScoreMultiplierManager Instance { get; private set; }

    [Header("Multiplier Settings")]
    [SerializeField] private float[] multiplierTiers = { 1f, 2f, 3f, 4f, 5f };
    [SerializeField] private string increaseAnimationTrigger = "OnMultiplierIncrease";

    private TextMeshProUGUI multiplierText;
    private Animator multiplierAnimator;

    private int currentTierIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (GameHUDController.Instance != null)
        {
            multiplierText = GameHUDController.Instance.MultiplierText;
            multiplierAnimator = GameHUDController.Instance.MultiplierAnimator;
        }

        ReviveManager.OnReviveSuccess += ResetMultiplier;
        ResetMultiplier();
    }

    private void OnDestroy()
    {
        ReviveManager.OnReviveSuccess -= ResetMultiplier;
    }

    public void IncreaseMultiplier()
    {
        if (currentTierIndex < multiplierTiers.Length - 1)
        {
            currentTierIndex++;
            UpdateUI();

            if (multiplierAnimator != null && !string.IsNullOrEmpty(increaseAnimationTrigger))
            {
                multiplierAnimator.SetTrigger(increaseAnimationTrigger);
            }
        }
    }

    public void ResetMultiplier()
    {
        currentTierIndex = 0;
        UpdateUI();
    }

    public float GetCurrentMultiplier()
    {
        return multiplierTiers[currentTierIndex];
    }

    private void UpdateUI()
    {
        if (multiplierText != null)
        {
            multiplierText.text = $"x{GetCurrentMultiplier()}";
        }

        OnMultiplierChanged?.Invoke(GetCurrentMultiplier());
    }
}
