
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manages the UI for the game's settings menu.
/// </summary>
public class SettingsUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        if (settingsPanel == null) 
        {
            Debug.LogError("Settings Panel is not assigned in the inspector!");
            return;
        }
        
        settingsPanel.SetActive(false); // Start with the panel hidden

        SetupListeners();
        LoadCurrentSettings();
    }

    private void SetupListeners()
    {
        // --- Sliders ---
        masterVolumeSlider?.onValueChanged.AddListener(SettingsManager.Instance.SetMasterVolume);
        musicVolumeSlider?.onValueChanged.AddListener(SettingsManager.Instance.SetMusicVolume);
        sfxVolumeSlider?.onValueChanged.AddListener(SettingsManager.Instance.SetSfxVolume);

        // --- Dropdown ---
        graphicsDropdown?.onValueChanged.AddListener(SettingsManager.Instance.SetGraphicsQuality);

        // --- Buttons ---
        saveButton?.onClick.AddListener(SettingsManager.Instance.SaveAllSettings);
        closeButton?.onClick.AddListener(TogglePanel); // A button to close the panel
    }

    /// <summary>
    /// Populates the UI with the current settings from the SettingsManager.
    /// </summary>
    private void LoadCurrentSettings()
    {
        if (SettingsManager.Instance == null) return;

        // --- Sliders ---
        if (masterVolumeSlider != null) masterVolumeSlider.value = SettingsManager.Instance.MasterVolume;
        if (musicVolumeSlider != null) musicVolumeSlider.value = SettingsManager.Instance.MusicVolume;
        if (sfxVolumeSlider != null) sfxVolumeSlider.value = SettingsManager.Instance.SfxVolume;

        // --- Dropdown ---
        if (graphicsDropdown != null)
        {
            graphicsDropdown.ClearOptions();
            graphicsDropdown.AddOptions(new List<string>(QualitySettings.names));
            graphicsDropdown.value = SettingsManager.Instance.GraphicsQuality;
            graphicsDropdown.RefreshShownValue();
        }
    }

    /// <summary>
    /// Toggles the visibility of the settings panel.
    /// </summary>
    public void TogglePanel()
    {
        bool isActive = !settingsPanel.activeSelf;
        settingsPanel.SetActive(isActive);

        if (isActive)
        {
            LoadCurrentSettings(); // Refresh settings each time the panel is opened
            Time.timeScale = 0; // Pause game when settings are open
        }
        else
        {
            Time.timeScale = 1; // Resume game
        }
    }

    private void OnDestroy()
    {
        // Clean up listeners
        masterVolumeSlider?.onValueChanged.RemoveAllListeners();
        musicVolumeSlider?.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider?.onValueChanged.RemoveAllListeners();
        graphicsDropdown?.onValueChanged.RemoveAllListeners();
        saveButton?.onClick.RemoveAllListeners();
        closeButton?.onClick.RemoveAllListeners();
    }
}
