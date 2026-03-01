using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FusionUI : MonoBehaviour
{
    [SerializeField] private Image fusionIcon;
    [SerializeField] private Image fusionTimerRadial;
    [SerializeField] private TMP_Text fusionNameText;

    private Coroutine timerCoroutine;

    public void Show(FusionType fusionType, float duration)
    {
        gameObject.SetActive(true);
        // Here you would set the icon and name based on the fusion type
        // For now, we'll just use a placeholder name
        fusionNameText.text = fusionType.ToString();
        
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        timerCoroutine = StartCoroutine(UpdateTimer(duration));
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
    }

    private System.Collections.IEnumerator UpdateTimer(float duration)
    {
        float timer = duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            fusionTimerRadial.fillAmount = timer / duration;
            yield return null;
        }
    }
}
