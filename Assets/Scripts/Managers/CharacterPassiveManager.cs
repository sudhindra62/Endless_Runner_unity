using UnityEngine;

public class CharacterPassiveManager : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<CharacterPassiveManager>();
    }

    public void RecalculatePassives()
    {
        // In a real implementation, this would check the currently selected character
        // and apply their inherent passive abilities.
        Debug.Log("Recalculating passives from the selected character.");

        // Example: Apply a character-specific gravity reduction
        // ServiceLocator.Get<PlayerMovement>()?.ApplyGravityMultiplier("CharacterPassive_LowGravity", 0.9f);
    }
}
