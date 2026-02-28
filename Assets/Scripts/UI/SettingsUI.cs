
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the in-game settings panel, allowing players to adjust audio, graphics, and other options.
/// </summary>
public class SettingsUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject settingsPanel;

    [Header("UI Controls")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Toggle vibrationToggle;

    private void Start()
    {
        // Load and apply saved settings when the panel is first loaded
        LoadSettings();

        // Add listeners for UI control changes
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        vibrationToggle.onValueChanged.AddListener(OnVibrationToggleChanged);
    }

    /// <summary>
    /// Shows the settings panel.
    /// </summary>
    public void Show()
    {
        settingsPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the settings panel.
    /// </summary>
    public void Hide()
    {
        settingsPanel.SetActive(false);
    }

    private void OnMasterVolumeChanged(float value)
    {
        // Use a dedicated AudioManager to control the game's audio levels
        // AudioManager.Instance.SetMasterVolume(value);
        Debug.Log("Master volume set to: " + value);
    }

    private void OnVibrationToggleChanged(bool isOn)
    {
        // Use a dedicated service to manage haptic feedback
        // VibrationService.Instance.SetVibration(isOn);
        Debug.Log("Vibration settings changed to: " + isOn);
    }

    private void LoadSettings()
    {
        // Load saved settings from PlayerPrefs or a data manager
        // masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        // vibrationToggle.isOn = PlayerPrefs.GetInt("VibrationEnabled", 1) == 1;
    }

    private void SaveSettings()
    {
        // Save the current settings
        // PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        // PlayerPrefs.SetInt("VibrationEnabled", vibrationToggle.isOn ? 1 : 0);
        // PlayerPrefs.Save();
    }
}
