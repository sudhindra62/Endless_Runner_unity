using UnityEngine;
using System.Collections;

public class PlayerPowerUp : MonoBehaviour
{
    [Header("Shield")]
    public GameObject shieldVisual;
    public AudioClip shieldBreakSound;
    public float shieldDuration = 10f;
    public ShieldTimerUI shieldTimerUI;

    private AudioSource audioSource;
    private bool shieldActive;
    private Coroutine shieldCoroutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(false);
        }
    }

    public void ActivateShield()
    {
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(ShieldRoutine());

        MilestoneManager.Instance?.ReportShieldUse();
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
        {
            shieldVisual.SetActive(false);
        }

        if (audioSource != null && shieldBreakSound != null)
        {
            audioSource.PlayOneShot(shieldBreakSound);
        }

        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
            shieldCoroutine = null;
        }

        if (shieldTimerUI != null)
        {
            shieldTimerUI.StopCountdown();
        }
    }

    private IEnumerator ShieldRoutine()
    {
        shieldActive = true;
        if (shieldVisual != null)
        {
            shieldVisual.SetActive(true);
        }

        if (shieldTimerUI != null)
        {
            shieldTimerUI.StartCountdown(shieldDuration);
        }

        float timer = shieldDuration;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            if (shieldTimerUI != null)
            {
                shieldTimerUI.UpdateFillAmount(timer / shieldDuration);
            }
            yield return null;
        }

        BreakShield();
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
            CoinMagnet.Instance?.ActivateMagnet();
            Destroy(other.gameObject);
        }
    }
}
