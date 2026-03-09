
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : UIPanel
{
    public override UIPanelType PanelType => UIPanelType.Settings;

    [SerializeField] private Slider _musicVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private Button _backButton;

    private void Start()
    {
        _musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnMusicVolumeChanged(float value)
    {
        //AudioManager.Instance.SetMusicVolume(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        //AudioManager.Instance.SetSFXVolume(value);
    }

    private void OnBackButtonClicked()
    {
        GameUIManager.Instance.HidePanel(UIPanelType.Settings);
    }
}
