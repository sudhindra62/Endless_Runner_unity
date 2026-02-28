
using UnityEngine;
using System;

public class DailyLoginManager : MonoBehaviour
{
    public static DailyLoginManager Instance { get; private set; }

    private const string LastLoginKey = "LastLoginTime_Binary";
    private const string DailyLoginChestId = "daily_login_chest";

    private RewardChestManager _rewardChestManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        _rewardChestManager = ServiceLocator.Get<RewardChestManager>();
        if (_rewardChestManager == null)
        {
            Debug.LogError("RewardChestManager not found!");
            return;
        }
        CheckForDailyLogin();
    }

    public void CheckForDailyLogin()
    {
        DateTime lastLogin = GetLastLoginTime();
        DateTime now = DateTime.UtcNow;

        if (IsNewDay(lastLogin, now))
        {
            if (_rewardChestManager.IsChestReady(DailyLoginChestId))
            {
                _rewardChestManager.OpenChest(DailyLoginChestId);
                Debug.Log("Daily login chest awarded!");
                PlayerPrefs.SetString(LastLoginKey, now.ToBinary().ToString());
            }
        }
    }

    private bool IsNewDay(DateTime lastLogin, DateTime now)
    {
        return now.Date > lastLogin.Date;
    }

    private DateTime GetLastLoginTime()
    {
        string lastLoginString = PlayerPrefs.GetString(LastLoginKey, null);
        if (string.IsNullOrEmpty(lastLoginString))
        {
            return DateTime.MinValue;
        }

        long lastLoginBinary;
        if (long.TryParse(lastLoginString, out lastLoginBinary))
        {
            return DateTime.FromBinary(lastLoginBinary);
        }

        return DateTime.MinValue;
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}
