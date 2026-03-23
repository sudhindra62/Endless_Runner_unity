using UnityEngine;

public class GameDifficultyManager : MonoBehaviour
{
    public static GameDifficultyManager Instance { get; private set; }
    private float speedBoostMultiplier = 1f;
    
    private void Awake() { Instance = this; }

    public void SetSpeedBoostMultiplier(float multiplier)
    {
        speedBoostMultiplier = multiplier;
    }

    public float GetSpeedBoostMultiplier() => speedBoostMultiplier;
}
