
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class acts as a bridge to a remote configuration service (like Firebase Remote Config).
/// It fetches configuration values at the start of the application and provides a fallback mechanism
/// to ensure the game can function even if the remote service is unavailable.
/// </summary>
public class RemoteConfigBridge : Singleton<RemoteConfigBridge>
{
    private Dictionary<string, object> _config = new Dictionary<string, object>();

    protected override void Awake()
    {
        base.Awake();
        FetchConfig();
    }

    private void FetchConfig()
    {
        // In a real implementation, this would involve an asynchronous call to a remote service.
        // For this example, we'll populate the config with default values.
        _config["revive_gem_cost"] = 10;
        _config["interstitial_frequency"] = 3;
        _config["ad_reward_coins"] = 100;
        _config["gem_pack_price_display"] = "$4.99";
        _config["subscription_price_display"] = "$9.99/month";
        
        Debug.Log("Remote config fetched and cached.");
    }

    public T GetValue<T>(string key, T defaultValue)
    {
        if (_config.TryGetValue(key, out object value))
        { 
            return (T)value;
        }
        return defaultValue;
    }
}

/// <summary>
/// This provider is a safe, read-only wrapper around the RemoteConfigBridge.
/// It ensures that other parts of the game can access configuration values but cannot modify them.
/// This is a crucial security measure to prevent cheating and unauthorized changes to the game's economy.
/// </summary>
public class OwnerConfigProvider
{
    public static int ReviveGemCost => RemoteConfigBridge.Instance.GetValue("revive_gem_cost", 10);
    public static int InterstitialFrequency => RemoteConfigBridge.Instance.GetValue("interstitial_frequency", 3);
    public static int AdRewardCoins => RemoteConfigBridge.Instance.GetValue("ad_reward_coins", 100);
    public static string GemPackPriceDisplay => RemoteConfigBridge.Instance.GetValue("gem_pack_price_display", "$4.99");
    public static string SubscriptionPriceDisplay => RemoteConfigBridge.Instance.GetValue("subscription_price_display", "$9.99/month");
}
