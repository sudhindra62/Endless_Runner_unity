
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the UI display for the Flow Combo system.
/// Subscribes to events from the FlowComboManager to update its state.
/// </summary>
public class FlowComboUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI comboCountText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private Slider comboDecaySlider;
    [SerializeField] private Animator tierPulseAnimator;

    [Header("Settings")]
    [SerializeField] private float decayUpdateSpeed = 2.0f;
    [SerializeField] private string tierPulseTriggerName = "TierPulse";

    private void OnEnable()
    {
        // Subscribe to events from the combo manager.
        FlowComboManager.OnComboChanged += UpdateComboCount;
        FlowComboManager.OnComboMultiplierChanged += UpdateMultiplier;
        FlowComboManager.OnComboBroken += OnComboBroken;
        FlowComboManager.OnTierIncreased += OnTierIncreased;

        // Initialize UI state.
        comboCountText.gameObject.SetActive(false);
        multiplierText.gameObject.SetActive(false);
        comboDecaySlider.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks.
        FlowComboManager.OnComboChanged -= UpdateComboCount;
        FlowComboManager.OnComboMultiplierChanged -= UpdateMultiplier;
        FlowComboManager.OnComboBroken -= OnComboBroken;
        FlowComboManager.OnTierIncreased -= OnTierIncreased;
    }

    private void Update()
    {
        // Smoothly update the decay slider.
        if (comboDecaySlider.gameObject.activeSelf)
        {
            float targetValue = FlowComboManager.Instance.ComboCount > 0 ? 1 : 0;
            comboDecaySlider.value = Mathf.Lerp(comboDecaySlider.value, targetValue, Time.deltaTime * decayUpdateSpeed);
        }
    }

    private void UpdateComboCount(int count)
    {
        if (count > 0)
        {
            comboCountText.text = $"x{count}";
            comboCountText.gameObject.SetActive(true);
            comboDecaySlider.gameObject.SetActive(true);
        }
        else
        {
            comboCountText.gameObject.SetActive(false);
            comboDecaySlider.gameObject.SetActive(false);
        }
    }

    private void UpdateMultiplier(float multiplier)
    {
        if (multiplier > 1f)
        {
            multiplierText.text = $"{multiplier:F1}x MULTIPLIER";
            multiplierText.gameObject.SetActive(true);
        }
        else
        {
            multiplierText.gameObject.SetActive(false);
        }
    }

    private void OnComboBroken()
    {
        comboCountText.gameObject.SetActive(false);
        multiplierText.gameObject.SetActive(false);
        comboDecaySlider.gameObject.SetActive(false);
    }

    private void OnTierIncreased(float newMultiplier)
    {
        if (tierPulseAnimator != null)
        {
            tierPulseAnimator.SetTrigger(tierPulseTriggerName);
        }
    }
}
