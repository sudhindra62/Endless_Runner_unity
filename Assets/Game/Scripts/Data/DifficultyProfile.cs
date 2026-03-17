using UnityEngine;

namespace EndlessRunner.Data
{
    [CreateAssetMenu(fileName = "DifficultyProfile", menuName = "Endless Runner/Difficulty Profile", order = 2)]
    public class DifficultyProfile : ScriptableObject
    {
        public string profileName;
        
        [Range(0f, 1f)]
        [Tooltip("The probability of an obstacle spawning in any given slot.")]
        public float obstacleDensity = 0.3f;
        
        [Range(0f, 1f)]
        [Tooltip("The probability of a coin path being activated.")]
        public float coinFrequency = 0.5f;
    }
}
