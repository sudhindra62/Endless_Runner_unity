using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private PlayerDeathHandler deathHandler;

    private void Awake()
    {
        deathHandler = GetComponent<PlayerDeathHandler>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            deathHandler?.Die();
        }
    }
}
