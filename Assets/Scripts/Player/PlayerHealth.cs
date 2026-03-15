
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static event Action OnPlayerDied;

    // This is a placeholder. In a real game, you would have health logic here.
    public void TakeDamage(int amount)
    {
        // For now, any damage kills the player.
        OnPlayerDied?.Invoke();
    }
}
