
using UnityEngine;

/// <summary>
/// A placeholder singleton for managing user consent (e.g., for GDPR).
/// It uses PlayerPrefs to store the user's consent status.
/// This provides the backend logic required before a UI dialog is implemented.
/// </summary>
public class ConsentManager : MonoBehaviour
{
    public static ConsentManager Instance { get; private set; }

    private const string ConsentKey = "Legal_ConsentAccepted";

    private bool? _hasConsentCache = null;

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
        }
    }

    /// <summary>
    /// Call this when the player accepts the consent agreement.
    /// </summary>
    public void AcceptConsent()
    {
        PlayerPrefs.SetInt(ConsentKey, 1); // 1 = true
        PlayerPrefs.Save();
        _hasConsentCache = true;
        Debug.Log("Consent has been accepted and saved.");
        // FUTURE HOOK: Initialize analytics, ads SDKs, etc., that require consent.
    }

    /// <summary>
    /// Checks if the user has previously accepted consent.
    /// </summary>
    public bool HasConsent()
    {
        if (_hasConsentCache.HasValue)
        {
            return _hasConsentCache.Value;
        }

        _hasConsentCache = PlayerPrefs.GetInt(ConsentKey, 0) == 1;
        return _hasConsentCache.Value;
    }
}
