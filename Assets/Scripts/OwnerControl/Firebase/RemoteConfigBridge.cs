using UnityEngine;
using EndlessRunner.OwnerControl;

#if FIREBASE_REMOTE_CONFIG
using Firebase.RemoteConfig;
using System.Collections.Generic;
#endif

namespace EndlessRunner.OwnerControl.Firebase
{
    /// <summary>
    /// Optional Firebase Remote Config bridge.
    /// Compiles and runs even when Firebase is NOT present.
    /// </summary>
    public class RemoteConfigBridge : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            ApplyCachedValues();

#if FIREBASE_REMOTE_CONFIG
            FetchRemoteConfig();
#endif
        }

        private void ApplyCachedValues()
        {
            OwnerConfigProvider.SetReviveGemCost(
                RemoteConfigCache.LoadInt(
                    OwnerConfigKeys.ReviveGemCost,
                    OwnerConfigDefaults.DefaultReviveGemCost));

            OwnerConfigProvider.SetReviveAdEnabled(
                RemoteConfigCache.LoadBool(
                    OwnerConfigKeys.ReviveAdEnabled,
                    OwnerConfigDefaults.DefaultReviveAdEnabled));

            OwnerConfigProvider.SetInterstitialFrequency(
                RemoteConfigCache.LoadInt(
                    OwnerConfigKeys.InterstitialFrequency,
                    OwnerConfigDefaults.DefaultInterstitialFrequency));

            OwnerConfigProvider.SetAdRewardCoins(
                RemoteConfigCache.LoadInt(
                    OwnerConfigKeys.AdRewardCoins,
                    OwnerConfigDefaults.DefaultAdRewardCoins));

            OwnerConfigProvider.SetAdsEnabled(
                RemoteConfigCache.LoadBool(
                    OwnerConfigKeys.AdsEnabled,
                    OwnerConfigDefaults.DefaultAdsEnabled));
        }

#if FIREBASE_REMOTE_CONFIG
        private void FetchRemoteConfig()
        {
            var defaults = new Dictionary<string, object>
            {
                { OwnerConfigKeys.ReviveGemCost, OwnerConfigDefaults.DefaultReviveGemCost },
                { OwnerConfigKeys.ReviveAdEnabled, OwnerConfigDefaults.DefaultReviveAdEnabled },
                { OwnerConfigKeys.InterstitialFrequency, OwnerConfigDefaults.DefaultInterstitialFrequency },
                { OwnerConfigKeys.AdRewardCoins, OwnerConfigDefaults.DefaultAdRewardCoins },
                { OwnerConfigKeys.AdsEnabled, OwnerConfigDefaults.DefaultAdsEnabled }
            };

            FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults);

            FirebaseRemoteConfig.DefaultInstance.FetchAsync()
                .ContinueWith(task =>
                {
                    if (!task.IsCompleted || task.IsFaulted)
                        return;

                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                        .ContinueWith(_ => ApplyRemoteValues());
                });
        }

        private void ApplyRemoteValues()
        {
            var config = FirebaseRemoteConfig.DefaultInstance;

            ApplyInt(OwnerConfigKeys.ReviveGemCost,
                (int)config.GetValue(OwnerConfigKeys.ReviveGemCost).LongValue,
                OwnerConfigProvider.SetReviveGemCost);

            ApplyBool(OwnerConfigKeys.ReviveAdEnabled,
                config.GetValue(OwnerConfigKeys.ReviveAdEnabled).BooleanValue,
                OwnerConfigProvider.SetReviveAdEnabled);

            ApplyInt(OwnerConfigKeys.InterstitialFrequency,
                (int)config.GetValue(OwnerConfigKeys.InterstitialFrequency).LongValue,
                OwnerConfigProvider.SetInterstitialFrequency);

            ApplyInt(OwnerConfigKeys.AdRewardCoins,
                (int)config.GetValue(OwnerConfigKeys.AdRewardCoins).LongValue,
                OwnerConfigProvider.SetAdRewardCoins);

            ApplyBool(OwnerConfigKeys.AdsEnabled,
                config.GetValue(OwnerConfigKeys.AdsEnabled).BooleanValue,
                OwnerConfigProvider.SetAdsEnabled);
        }

        private void ApplyInt(string key, int value, System.Action<int> setter)
        {
            setter(Mathf.Max(0, value));
            RemoteConfigCache.SaveInt(key, value);
        }

        private void ApplyBool(string key, bool value, System.Action<bool> setter)
        {
            setter(value);
            RemoteConfigCache.SaveBool(key, value);
        }
#endif
    }
}
