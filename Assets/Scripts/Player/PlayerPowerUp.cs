using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    [Header("Shield")]
    public GameObject shieldVisual;
    public AudioClip shieldBreakSound;

    private AudioSource audioSource;
    private bool shieldActive;

    public float shieldDuration = 10f;
    public ShieldTimerUI shieldTimerUI;


    void Awake()
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

            Invoke(nameof(BreakShield), shieldDuration);
            shieldTimerUI.StartTimer(shieldDuration);

    }

    public bool HasShield()
    {
        return shieldActive;
    }

    public void BreakShield()
    {
        shieldActive = false;

        if (shieldVisual != null)
            shieldVisual.SetActive(false);

        if (audioSource && shieldBreakSound)
            audioSource.PlayOneShot(shieldBreakSound);

        CancelInvoke(nameof(BreakShield));
shieldTimerUI.StopTimer();


#if UNITY_ANDROID
        Handheld.Vibrate();
#endif
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Shield"))
        {
            ActivateShield();
            Destroy(other.gameObject);
        }
    }
}
