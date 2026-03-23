
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpFusionUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image screenGlowImage;
    [SerializeField] private Image fusionIconImage;
    [SerializeField] private TextMeshProUGUI fusionCountdownText;
    [SerializeField] private AudioSource fusionSoundPlayer;

    [Header("Fusion Visuals")]
    [SerializeField] private Sprite coinStormIcon;
    [SerializeField] private Sprite invincibleDashIcon;
    [SerializeField] private Sprite feverFrenzyIcon;
    [SerializeField] private Color coinStormGlow = Color.yellow;
    [SerializeField] private Color invincibleDashGlow = Color.cyan;
    [SerializeField] private Color feverFrenzyGlow = Color.magenta;

    [Header("Fusion Sounds")]
    [SerializeField] private AudioClip fusionActivationSound;

    private Coroutine activeFusionCoroutine;

    private void Start()
    {
        if (PowerUpFusionManager.Instance != null)
        {
            PowerUpFusionManager.Instance.OnFusionActivated += HandleFusionActivation;
            PowerUpFusionManager.Instance.OnFusionDeactivated += HandleFusionDeactivation;
        }
        SetUIVisible(false);
    }

    private void OnDestroy()
    {
        if (PowerUpFusionManager.Instance != null) {
            PowerUpFusionManager.Instance.OnFusionActivated -= HandleFusionActivation;
            PowerUpFusionManager.Instance.OnFusionDeactivated -= HandleFusionDeactivation;
        }
    }

    private void HandleFusionActivation(FusionModifierData data)
    {
        if (activeFusionCoroutine != null)
        {
            StopCoroutine(activeFusionCoroutine);
        }
        activeFusionCoroutine = StartCoroutine(FusionCountdownRoutine(data));
        
        if (fusionSoundPlayer != null && fusionActivationSound != null)
        {
            fusionSoundPlayer.PlayOneShot(fusionActivationSound);
        }
    }

    private void HandleFusionDeactivation()
    { 
        if(activeFusionCoroutine != null)
        {
            StopCoroutine(activeFusionCoroutine);
            activeFusionCoroutine = null;
        }
        SetUIVisible(false);
    }

    private System.Collections.IEnumerator FusionCountdownRoutine(FusionModifierData data)
    {
        Sprite icon = GetFusionIcon(data.Type);
        Color glow = GetFusionGlowColor(data.Type);
        
        fusionIconImage.sprite = icon;
        screenGlowImage.color = glow;
        SetUIVisible(true);

        float remainingTime = data.Duration;
        while (remainingTime > 0)
        {
            fusionCountdownText.SetText(Mathf.CeilToInt(remainingTime).ToString());
            remainingTime -= Time.deltaTime;
            yield return null;
        }

        SetUIVisible(false);
    }

    private void SetUIVisible(bool isVisible)
    {
        screenGlowImage.enabled = isVisible;
        fusionIconImage.enabled = isVisible;
        fusionCountdownText.enabled = isVisible;
    }

    private Sprite GetFusionIcon(FusionType type)
    {
        switch (type)
        {
            case FusionType.CoinStorm: return coinStormIcon;
            case FusionType.InvincibleDash: return invincibleDashIcon;
            case FusionType.FeverFrenzy: return feverFrenzyIcon;
            default: return null;
        }
    }

    private Color GetFusionGlowColor(FusionType type)
    {
        switch (type)
        {
            case FusionType.CoinStorm: return coinStormGlow;
            case FusionType.InvincibleDash: return invincibleDashGlow;
            case FusionType.FeverFrenzy: return feverFrenzyGlow;
            default: return Color.clear;
        }
    }
}
