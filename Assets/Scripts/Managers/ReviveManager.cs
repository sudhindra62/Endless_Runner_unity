
using UnityEngine;
using System;

/// <summary>
/// Manages the player revive process, including currency transactions and game state coordination.
/// Reconstructed by OMNI_LOGIC_COMPLETION_v2 for full functionality.
/// </summary>
public class ReviveManager : MonoBehaviour
{
    public static ReviveManager Instance { get; private set; }

    public static event Action OnRevivePrompt;
    public static event Action OnReviveSuccess;
    public static event Action OnReviveDecline;

    [Tooltip("The cost in gems for the first revive.")]
    [SerializeField] private int baseReviveCost = 10;
    [Tooltip("How much the cost increases after each revive.")]
    [SerializeField] private int costMultiplier = 2;

    private int timesRevivedThisRun = 0;
    private bool canRevive = true;

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

    private void Start()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameManager.GameState newState)
    {
        // Reset revive counter when a new game starts
        if (newState == GameManager.GameState.Playing)
        {
            timesRevivedThisRun = 0;
            canRevive = true;
        }
    }

    public bool CanRevive()
    {
        // Add any other conditions for revival (e.g., once per run)
        return canRevive;
    }

    public void PromptRevive()
    {
        if (!CanRevive()) return;

        Debug.Log("Prompting player to revive.");
        OnRevivePrompt?.Invoke();
        // This event will be caught by a UI Manager to show the revive popup.
    }

    public int GetCurrentReviveCost()
    {
        return baseReviveCost * (int)Mathf.Pow(costMultiplier, timesRevivedThisRun);
    }

    public void AttemptRevive()
    {
        if (!canRevive) return;

        int cost = GetCurrentReviveCost();
        bool success = false;

        if (CurrencyManager.Instance != null)
        {
            success = CurrencyManager.Instance.TrySpendGems(cost);
        }

        if (success)
        {
            HandleReviveSuccess();
        }
        else
        {
            Debug.Log("Not enough gems to revive.");
            HandleReviveDecline();
        }
    }
    
    public void AttemptReviveWithAd()
    {
        if (!canRevive) return;

        GameManager.Instance.Ads.ShowRewardedVideo(success =>
        {
            if (success)
            {
                HandleReviveSuccess();
            }
            else
            {
                Debug.Log("Ad not shown, revive failed.");
                HandleReviveDecline();
            }
        });
    }

    public void DeclineRevive()
    {
        HandleReviveDecline();
    }

    private void HandleReviveSuccess()
    {
        timesRevivedThisRun++;
        canRevive = false; // Simple logic: only one revive per run. Can be changed.

        Debug.Log("Revive successful!");
        OnReviveSuccess?.Invoke();

        // The GameFlowController will catch this and set the state back to Playing
        if (GameFlowController.Instance != null)
        {
            GameFlowController.Instance.ReviveAccepted();
        }
        
        // We need to make the player active again and maybe give temporary invincibility
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.SetActive(true);
            // It's good practice to give a few seconds of invincibility
            // player.GetComponent<PlayerHealth>().SetInvincible(3.0f);
        }
    }

    private void HandleReviveDecline()
    {
        canRevive = false;
        Debug.Log("Revive declined.");
        OnReviveDecline?.Invoke();

        // Tell the GameManager it's officially game over
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SetState(GameManager.GameState.GameOver);
        }
    }
}
