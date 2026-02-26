using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShieldTimerUI : MonoBehaviour
{
    public Image fillImage;
    private Coroutine countdownCoroutine;

    public void StartCountdown(float duration)
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(Countdown(duration));
    }

    public void StopCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        fillImage.fillAmount = 0;
    }

    public void UpdateFillAmount(float fillAmount)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = fillAmount;
        }
    }

    private IEnumerator Countdown(float duration)
    {
        float timer = duration;
        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            UpdateFillAmount(timer / duration);
            yield return null;
        }
        UpdateFillAmount(0);
    }
}
