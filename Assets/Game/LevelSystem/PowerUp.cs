using UnityEngine;

namespace EndlessRunner.Level
{
    public class PowerUp : MonoBehaviour
    {
        public float speedBoost = 5f;
        public float duration = 5f;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var playerController = other.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.ActivateSpeedBoost(speedBoost, duration);
                    // Play power-up collection sound
                    Destroy(gameObject);
                }
            }
        }
    }
}
