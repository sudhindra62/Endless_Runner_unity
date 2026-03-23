
using UnityEngine;
using System;
using System.Collections.Generic;

public class SpinWheelManager : Singleton<SpinWheelManager>
{
    [Header("Spin Wheel Settings")]
    [SerializeField] private List<SpinWheelReward> rewards;
    [SerializeField] private AnimationCurve spinAnimationCurve;
    [SerializeField] private float spinDuration = 3f;
    [SerializeField] private int adSpinsPerDay = 1;

    [Header("UI")]
    [SerializeField] private Transform wheelTransform;
    [SerializeField] private GameObject spinButton;
    [SerializeField] private GameObject adSpinButton;

    private bool isSpinning = false;
    private int adSpinsUsedToday;
    private DateTime lastFreeSpinTime;

    private void Start()
    {
        LoadSpinData();
        UpdateSpinButtons();
    }

    public void AttemptSpin()
    {
        if (isSpinning) return;
        if (!IntegrityManager.Instance.IsSecure()) return;

        if (CanFreeSpin())
        {
            StartSpin();
            lastFreeSpinTime = DateTime.UtcNow;
            SaveSpinData();
        }
    }

    public void AttemptAdSpin()
    {
        if (isSpinning) return;
        if (!IntegrityManager.Instance.IsSecure()) return;

        if (adSpinsUsedToday < adSpinsPerDay)
        {
            // In a real implementation, you would show an ad here.
            // For now, we'''ll just grant the spin.
            StartSpin();
            adSpinsUsedToday++;
            SaveSpinData();
        }
    }

    private void StartSpin()
    {
        isSpinning = true;
        UpdateSpinButtons();

        int rewardIndex = GetWeightedRandomRewardIndex();
        float targetAngle = 360f * 5 + (360f / rewards.Count) * rewardIndex;

        StartCoroutine(SpinTheWheel(targetAngle, () => {
            RewardManager.Instance.GrantReward(rewards[rewardIndex].rewardType, rewards[rewardIndex].amount);
            isSpinning = false;
            UpdateSpinButtons();
        }));
    }

    private System.Collections.IEnumerator SpinTheWheel(float targetAngle, Action onComplete)
    {
        float startAngle = wheelTransform.eulerAngles.z;
        float time = 0;

        while (time < spinDuration)
        {
            time += Time.deltaTime;
            float angle = Mathf.Lerp(startAngle, targetAngle, spinAnimationCurve.Evaluate(time / spinDuration));
            wheelTransform.eulerAngles = new Vector3(0, 0, angle);
            yield return null;
        }

        onComplete?.Invoke();
    }

    private int GetWeightedRandomRewardIndex()
    {
        float totalWeight = 0;
        foreach (var reward in rewards)
        {
            totalWeight += reward.weight;
        }

        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float cumulativeWeight = 0;

        for (int i = 0; i < rewards.Count; i++)
        {
            cumulativeWeight += rewards[i].weight;
            if (randomValue < cumulativeWeight)
            {
                return i;
            }
        }
        return rewards.Count - 1;
    }

    private bool CanFreeSpin()
    {
        return (DateTime.UtcNow - lastFreeSpinTime).TotalHours >= 24;
    }

    private void UpdateSpinButtons()
    {
        spinButton.SetActive(CanFreeSpin() && !isSpinning);
        adSpinButton.SetActive(adSpinsUsedToday < adSpinsPerDay && !isSpinning);
    }

    private void LoadSpinData()
    {
        lastFreeSpinTime = new DateTime(SaveManager.Instance.LoadLastSpinTime());
        adSpinsUsedToday = SaveManager.Instance.LoadAdSpinsUsed();
    }

    private void SaveSpinData()
    {
        SaveManager.Instance.SaveLastSpinTime(lastFreeSpinTime.Ticks);
        SaveManager.Instance.SaveAdSpinsUsed(adSpinsUsedToday);
    }

    // This would be called daily, perhaps from the DailyLoginManager
    public void ResetDailySpins()
    {
        adSpinsUsedToday = 0;
        SaveSpinData();
        UpdateSpinButtons();
    }
}

[System.Serializable]
public class SpinWheelReward
{
    public string rewardType; // e.g., "coins", "gems", "shield"
    public int amount;
    public float weight;
}
