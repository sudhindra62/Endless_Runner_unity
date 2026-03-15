
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    public Transform playerTransform;
    public float moveSpeed = 5f;
    public float despawnDistance = 50f;

    private void Update()
    {
        if (playerTransform == null) return;

        // Move towards the player
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

        // Despawn if too far from the player
        if (Vector3.Distance(transform.position, playerTransform.position) > despawnDistance)
        {
            Destroy(gameObject);
        }
    }
}
