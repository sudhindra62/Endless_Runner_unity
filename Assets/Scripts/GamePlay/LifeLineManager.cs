using UnityEngine;
using System;

/// <summary>
/// Manages the activation of in-game lifeline effects.
/// </summary>
public class LifeLineManager : MonoBehaviour
{
    public static LifeLineManager Instance { get; private set; }

    // --- Existing Events ---
    public static event Action OnShieldActivated;
    public static event Action<float> OnSlowMotionActivated;
    public static event Action OnAutoJumpActivated;

    // 🔹 COMPATIBILITY EVENT — UI EXPECTS THIS
    public static event Action OnLifeLineTriggered;

    [Header("Effect Settings")]
    [SerializeField] private float slowMotionDuration = 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void TryActivateLifeLine(LifeLineType type)
    {
        if (LifeLineInventoryManager.Instance.UseLifeLine(type))
        {
            ActivateEffect(type);
            OnLifeLineTriggered?.Invoke();
        }
    }

    private void ActivateEffect(LifeLineType type)
    {
        switch (type)
        {
            case LifeLineType.Shield:
                OnShieldActivated?.Invoke();
                break;

            case LifeLineType.SlowMotionEscape:
                OnSlowMotionActivated?.Invoke(slowMotionDuration);
                break;

            case LifeLineType.AutoJumpRescue:
                OnAutoJumpActivated?.Invoke();
                break;

            case LifeLineType.ReviveOnce:
                break;
        }
    }

    // 🔹 COMPATIBILITY METHODS — UI CALLS THESE
    public void UseRevive()
    {
        OnLifeLineTriggered?.Invoke();
    }

    public void ConfirmGameOver()
    {
        // Intentionally empty — behavior handled elsewhere
    }
}
