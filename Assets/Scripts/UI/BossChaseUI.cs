
using UnityEngine;
using UnityEngine.UI;

public class BossChaseUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject warningIndicator;
    [SerializeField] private Slider timerBar;
    [SerializeField] private Image screenEdgePulse;
    [SerializeField] private Text rewardNotification;

    private void Start()
    {
        BossChaseManager.OnBossChaseStart += OnBossChaseStart;
        BossChaseManager.OnBossChaseEnd += OnBossChaseEnd;

        if (warningIndicator != null) warningIndicator.SetActive(false);
        if (timerBar != null) timerBar.gameObject.SetActive(false);
        if (screenEdgePulse != null) screenEdgePulse.gameObject.SetActive(false);
        if (rewardNotification != null) rewardNotification.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        BossChaseManager.OnBossChaseStart -= OnBossChaseStart;
        BossChaseManager.OnBossChaseEnd -= OnBossChaseEnd;
    }

    private void Update()
    {
        if(BossChaseManager.Instance != null && BossChaseManager.Instance.RemainingChaseTime > 0)
        {
            if(timerBar != null)
            {
                timerBar.value = BossChaseManager.Instance.RemainingChaseTime;
            }
        }
    }

    private void OnBossChaseStart(float chaseDuration)
    {
        if (warningIndicator != null) StartCoroutine(FlashWarning());
        if (timerBar != null) 
        {
            timerBar.gameObject.SetActive(true);
            timerBar.maxValue = chaseDuration;
            timerBar.value = chaseDuration;
        }
        if (screenEdgePulse != null) screenEdgePulse.gameObject.SetActive(true); //This would be animated

        // Music change logic would go here
    }

    private void OnBossChaseEnd()
    {
        if (warningIndicator != null) warningIndicator.SetActive(false);
        if (timerBar != null) timerBar.gameObject.SetActive(false);
        if (screenEdgePulse != null) screenEdgePulse.gameObject.SetActive(false);
    }

    public void ShowRewardNotification(string message)
    {
        if (rewardNotification != null)
        {
            rewardNotification.text = message;
            rewardNotification.gameObject.SetActive(true);
            // Hide after a few seconds
            Invoke(nameof(HideRewardNotification), 3f); 
        }
    }

    private void HideRewardNotification()
    {
        if (rewardNotification != null) rewardNotification.gameObject.SetActive(false);
    }

    private System.Collections.IEnumerator FlashWarning()
    {
        if (warningIndicator == null) yield break;

        for(int i = 0; i < 3; i++)
        {
            warningIndicator.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            warningIndicator.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
