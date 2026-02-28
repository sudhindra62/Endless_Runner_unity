
using UnityEngine;
using System.Collections;

public class PlayerPowerUpHandler : MonoBehaviour
{
    [Header("Shield")]
    [SerializeField] private float shieldDuration = 10f;
    [SerializeField] private GameObject shieldVisual;
    [SerializeField] private AudioClip shieldBreakSound;

    [Header("UI")]
    [SerializeField] private ShieldTimerUI shieldTimerUI;

    private bool shieldActive = false;
    private Coroutine shieldCoroutine;
    private AudioSource audioSource;
    private CoinMagnet coinMagnet;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        coinMagnet = FindFirstObjectByType<CoinMagnet>();
        if (shieldVisual != null) shieldVisual.SetActive(false);
    }

    public void ActivateShield()
    {
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(ShieldRoutine());
    }

    public void ActivateMagnet()
    {
        if (coinMagnet != null)
        {
            coinMagnet.ActivateMagnet();
        }
    }

    public void BreakShield()
    {
        if (!shieldActive) return;

        shieldActive = false;
        if (shieldVisual != null) shieldVisual.SetActive(false);
        if (audioSource != null && shieldBreakSound != null) audioSource.PlayOneShot(shieldBreakSound);

        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
            shieldCoroutine = null;
        }
        shieldTimerUI?.StopCountdown();
    }

    public bool IsShieldActive()
    {
        return shieldActive;
    }

    private IEnumerator ShieldRoutine()
    {
        shieldActive = true;
        if (shieldVisual != null) shieldVisual.SetActive(true);
        shieldTimerUI?.StartCountdown(shieldDuration);

        yield return new WaitForSeconds(shieldDuration);

        if (shieldActive)
        {
            BreakShield();
        }
    }
}
