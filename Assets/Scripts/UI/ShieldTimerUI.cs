using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShieldTimerUI : MonoBehaviour
{
    [Header("UI References")]
    public Image timerImage;

    private Coroutine countdownCoroutine;

    public void StartCountdown(float duration)
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        countdownCoroutine = StartCoroutine(CountdownRoutine(duration));
    }

    public void StopCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
        if (timerImage != null)
        {
            timerImage.fillAmount = 0;
        }
    }

    private IEnumerator CountdownRoutine(float duration)
    {
        if (timerImage == null) yield break;

        float timer = duration;
        timerImage.fillAmount = 1f;

        while (timer > 0)
        {
            timer -= Time.unscaledDeltaTime;
            timerImage.fillAmount = timer / duration;
            yield return null;
        }

        timerImage.fillAmount = 0;
        countdownCoroutine = null;
    }
}
