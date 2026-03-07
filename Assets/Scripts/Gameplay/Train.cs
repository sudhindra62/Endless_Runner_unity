
using UnityEngine;

public class Train : MonoBehaviour
{
    [Header("Movement")]
    [Tooltip("The speed at which the train moves towards the player.")]
    public float movementSpeed = 20f;

    [Header("Spawning")]
    [Tooltip("The tag used by the ObjectPooler for this train type.")]
    public string poolTag = "Train";

    void Update()
    {
        // Move the train forward in its local Z direction.
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Example: If the train collides with the player, the game is over.
        if (collision.gameObject.CompareTag("Player"))
        {
            // In a real game, this would be handled by a GameManager.
            Debug.Log("Game Over!");
            Time.timeScale = 0; // Pause the game.
        }
    }

    private void BecameInvisible()
    {
        // If the train is no longer visible to any camera, return it to the pool.
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        // In a real implementation, you would use an object pooler.
        gameObject.SetActive(false);
    }
}
