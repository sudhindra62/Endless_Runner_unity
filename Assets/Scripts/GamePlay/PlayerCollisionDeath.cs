using UnityEngine;

public class PlayerCollisionDeath : MonoBehaviour
{
    private PlayerDeathHandler deathHandler;

    private void Awake()
    {
        deathHandler = GetComponentInParent<PlayerDeathHandler>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Player hit obstacle");
            deathHandler.Die();
        }
    }
}
