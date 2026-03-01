using UnityEngine;

public class SkillTreeManager : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<SkillTreeManager>();
    }

    public void RecalculatePassives()
    {
        // In a real implementation, this would read from the player's saved data
        // and apply any permanent bonuses they have unlocked.
        Debug.Log("Recalculating passives from the skill tree.");

        // Example: Apply a permanent speed boost from the skill tree
        // ServiceLocator.Get<PlayerMovement>()?.ApplySpeedMultiplier("SkillTree_SpeedBoost", 1.1f);
    }
}
