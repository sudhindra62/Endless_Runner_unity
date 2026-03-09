
using UnityEngine;

namespace PowerUps
{
    public abstract class PowerUp : MonoBehaviour
    {
        public float duration = 10f;

        public abstract void ApplyEffect();
    }
}
