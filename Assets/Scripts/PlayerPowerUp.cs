using UnityEngine;

/// <summary>
/// Unified PlayerPowerUp
/// Preserves:
/// - Shield system
/// - Timer UI
/// - Audio
/// - Vibration
/// - Milestone tracking
/// - Magnet powerup
/// </summary>
public class PlayerPowerUp : MonoBehaviour
{
    [Header("Shield")]
    public GameObject shieldVisual;
    public AudioClip shieldBreakSound;

    public float shieldDuration = 10f;
    public ShieldTimerUI shieldTimerUI;

    private AudioSource audioSource;
    private bool shieldActive;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (shieldVisual != null)
            shieldVisual.SetActive(false);
    }

    public void ActivateShield()
    {
        shieldActive = true;

        if (shieldVisual != null)
            shieldVisual.SetActive(true);

        // Report milestone (if system exists)
        MilestoneManager.Instance?.ReportShieldsUsed();

        CancelInvoke(nameof(BreakShield));
        Invoke(nameof(BreakShield), shieldDuration);

        if (shieldTimerUI != null)
            shieldTimerUI.StartTimer(shieldDuration);
    }

    public bool HasShield()
    {
        return shieldActive;
    }

    public void BreakShield()
    {
        if (!shieldActive) return;

        shieldActive = false;

        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        if (audioSource != null && shieldBreakSound != null)
            audioSource.PlayOneShot(shieldBreakSound);

        CancelInvoke(nameof(BreakShield));

        if (shieldTimerUI != null)
            shieldTimerUI.StopTimer();

#if UNITY_ANDROID
        Handheld.Vibrate();
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shield"))
        {
            ActivateShield();
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Magnet"))
        {
            MagnetUpgradeManager.Instance?.ActivateMagnet(MagnetTier.Small);
            Destroy(other.gameObject);
        }
    }
}
