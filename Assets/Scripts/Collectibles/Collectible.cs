
using UnityEngine;
using System.Collections;
using PowerUps;

/// <summary>
/// A base class for all collectible items in the game.
/// </summary>
public class Collectible : MonoBehaviour
{
    [Tooltip("The tag used by the ObjectPooler for this collectible type.")]
    [SerializeField] protected string poolTag;
    [SerializeField] private float attractionSpeed = 15f;

    [Tooltip("The power-up to grant when collected. Can be null for simple collectibles like coins.")]
    [SerializeField] private PowerUp powerUpToGrant;

    protected ObjectPooler objectPooler;
    private Transform targetPlayer;
    private bool isAttracted = false;
    private Coroutine attractionCoroutine;

    protected virtual void Start()
    {
        objectPooler = ServiceLocator.Get<ObjectPooler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollect();
            ReturnToPool();
        }
    }

    public void Attract(Transform playerTransform)
    {
        if (!isAttracted)
        {
            targetPlayer = playerTransform;
            isAttracted = true;
            if (attractionCoroutine != null) StopCoroutine(attractionCoroutine);
            attractionCoroutine = StartCoroutine(MoveTowardsPlayer());
        }
    }

    private IEnumerator MoveTowardsPlayer()
    {
        while (targetPlayer != null && Vector3.Distance(transform.position, targetPlayer.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPlayer.position, Time.deltaTime * attractionSpeed);
            yield return null;
        }
        // Once close enough, snap to the player and collect
        OnCollect();
        ReturnToPool();
    }

    protected virtual void OnCollect()
    {
        if (powerUpToGrant != null)
        {
            PowerUpManager.Instance.ActivatePowerUp(powerUpToGrant);
        }
    }

    private void ReturnToPool()
    {
        // Reset state before returning to the pool
        isAttracted = false;
        targetPlayer = null;
        if (attractionCoroutine != null)
        {
            StopCoroutine(attractionCoroutine);
            attractionCoroutine = null;
        }

        if (objectPooler != null)
        { 
            objectPooler.ReturnToPool(poolTag, gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnDisable()
    {
        // Ensure coroutine is stopped when the object is disabled (e.g., by the pool)
        isAttracted = false;
        targetPlayer = null;
        if (attractionCoroutine != null)
        {
            StopCoroutine(attractionCoroutine);
            attractionCoroutine = null;
        }
    }
}
