
using UnityEngine;

namespace EndlessRunner.Core
{
    public enum PowerUpType
    {
        None,
        SpeedBoost,
        DoubleJump,
        Invincibility,
        CoinMagnet,
        ScoreMultiplier
    }

    [CreateAssetMenu(fileName = "New PowerUp", menuName = "Endless Runner/Power-Up Definition")]
    public class PowerUpDefinition : ScriptableObject
    {
        [Header("Core Properties")]
        public PowerUpType type = PowerUpType.None;
        public float duration = 10f;
        public float value = 0f; // e.g., speed multiplier, number of jumps, score multiplier factor

        [Header("Visual & Audio Feedback")]
        public GameObject activationEffect;
        public AudioClip activationSound;
    }
}
