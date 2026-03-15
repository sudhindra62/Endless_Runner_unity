
using UnityEngine;

namespace EndlessRunner.PowerUps
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

        [Header("UI Properties")]
        public Sprite icon;

        [Header("Visual & Audio Feedback")]
        public GameObject activationEffect;
        public AudioClip activationSound;

        private float remainingDuration;

        public void OnEnable()
        {
            remainingDuration = 0;
        }

        public void Activate()
        {
            remainingDuration = duration;
        }
        
        public void Tick(float deltaTime)
        {
            remainingDuration -= deltaTime;
        }

        public bool IsActive()
        {
            return remainingDuration > 0;
        }

        public float GetRemainingDuration()
        {
            return remainingDuration;
        }
    }
}
