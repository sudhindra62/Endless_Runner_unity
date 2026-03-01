using UnityEngine;

/// <summary>
/// Defines the type of skillful action this trigger represents.
/// </summary>
public enum SkillEventType
{
    NearMiss,
    CleanSlide,
    PerfectDodge
}

/// <summary>
/// Detects when a player performs a skillful action (e.g., a near miss or clean slide)
/// and rewards them by increasing their combo.
/// </summary>
[RequireComponent(typeof(Collider))]
public class SkillTrigger : MonoBehaviour
{
    [Header("Trigger Configuration")]
    [SerializeField] private SkillEventType eventType = SkillEventType.NearMiss;
    [SerializeField] private int comboValue = 1; // Amount of combo to add
    [SerializeField] private float feverGaugeValue = 5f; // Amount to add to the fever gauge
    [SerializeField] private bool disableOnTrigger = true; // Prevents the trigger from firing multiple times

    private FlowComboManager flowComboManager;
    private FeverModeManager feverModeManager;
    private PlayerController playerController;

    private bool hasBeenTriggered = false;

    private void Start()
    {
        // Resolve dependencies using the ServiceLocator
        flowComboManager = ServiceLocator.Get<FlowComboManager>();
        feverModeManager = ServiceLocator.Get<FeverModeManager>();
        playerController = ServiceLocator.Get<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered || !other.CompareTag("Player"))
        {
            return;
        }

        bool actionSuccessful = false;

        // Check if the player's state matches the required action
        switch (eventType)
        {
            case SkillEventType.CleanSlide:
                if (playerController.IsSliding())
                {
                    actionSuccessful = true;
                }
                break;
            
            case SkillEventType.NearMiss:
                // For a near miss, just being in the trigger zone is enough.
                // We can add more complex checks here later if needed (e.g., not sliding or jumping).
                actionSuccessful = true;
                break;

            case SkillEventType.PerfectDodge:
                // This would require a more complex check, potentially involving the player's lane change timing.
                // For now, we can treat it like a near miss.
                actionSuccessful = true;
                break;
        }

        if (actionSuccessful)
        {
            // Reward the player
            if (flowComboManager != null)
            {
                flowComboManager.AddCombo(comboValue);
            }

            if (feverModeManager != null)
            {
                feverModeManager.AddFeverPoints(feverGaugeValue);
            }

            // Mark as triggered and optionally disable the collider
            hasBeenTriggered = true;
            if (disableOnTrigger)
            {
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
