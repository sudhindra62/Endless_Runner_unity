
using UnityEngine;

namespace PowerUps
{
    public enum PowerUpType
    {
        CoinDoubler,
        Magnet,
        ScoreMultiplier,
        Shield,
    }

    public abstract class PowerUp : ScriptableObject
    {
        [Header("Configuration")]
        [SerializeField] private float duration;

        public float Duration => duration;

        public abstract PowerUpType Type { get; }

        public abstract void Activate();
        public abstract void Deactivate();
    }
}
