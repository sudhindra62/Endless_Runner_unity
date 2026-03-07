
using UnityEngine;

public class ReturnToPool : MonoBehaviour
{
    [SerializeField] private float despawnDistance = -10f;
    private Transform player;

    private void Start()
    {
        // This assumes the player has the "Player" tag.
        // For a more robust system, consider a direct reference or a manager.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        if (player != null && transform.position.z < player.position.z + despawnDistance)
        {
            ObjectPool.Instance.ReturnObjectToPool(gameObject);
        }
    }
}
