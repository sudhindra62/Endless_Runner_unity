using UnityEngine;

namespace EndlessRunner.Level
{
    public class Obstacle : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.GameOver();
            }
        }
    }
}
