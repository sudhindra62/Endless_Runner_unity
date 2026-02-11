using UnityEngine;
using UnityEngine.UI;

public class ShieldTimerUI : MonoBehaviour
{
    public Image fillImage;

    float duration;
    float timeLeft;
    bool running;

    public void StartTimer(float seconds)
    {
        duration = seconds;
        timeLeft = seconds;
        running = true;
        gameObject.SetActive(true);
    }

    public void StopTimer()
    {
        running = false;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!running) return;

        timeLeft -= Time.unscaledDeltaTime;
        fillImage.fillAmount = timeLeft / duration;

        if (timeLeft <= 0)
            StopTimer();
    }
}
