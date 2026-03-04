using UnityEngine;

public class CosmeticEffectManager : MonoBehaviour
{
    public void UnlockEffect(string effectID)
    {
        Debug.Log($"Cosmetic effect unlocked: {effectID}");
    }
}
