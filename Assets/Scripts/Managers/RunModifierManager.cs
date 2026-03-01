
using UnityEngine;
using System.Collections;

// This is a placeholder to demonstrate integration with World Events.
public class RunModifierManager : MonoBehaviour
{
    public static RunModifierManager Instance { get; private set; }

    private const string RUN_MOD_SOURCE_ID = "RunModifier";

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

    // Call this to simulate the player picking up a modifier during a run.
    public void ActivateTemporaryCoinBoost(float multiplier, float duration)
    {
        StartCoroutine(TemporaryBoostRoutine(multiplier, duration));
    }

    private IEnumerator TemporaryBoostRoutine(float multiplier, float duration)
    {
        Debug.Log($"Activating temporary run modifier: {multiplier}x coin boost for {duration} seconds.");
        DataManager.Instance.ApplyCoinMultiplier(RUN_MOD_SOURCE_ID, multiplier);

        yield return new WaitForSeconds(duration);

        Debug.Log("Temporary run modifier expired.");
        DataManager.Instance.RemoveCoinMultiplier(RUN_MOD_SOURCE_ID);
    }
}
