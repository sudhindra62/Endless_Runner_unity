using System.Collections;
using UnityEngine;

/// <summary>
/// Defines the behavior of a projectile.
/// Supports both object pooling and standard instantiation.
/// Global scope.
/// </summary>
public class Projectile : MonoBehaviour
{
    [Header("Projectile Configuration")]
    public float speed = 20f;
    public float lifetime = 5f;
    public int damage = 10;
    public string ownerTag;
    public string poolTag = "Projectile";

    private Coroutine _lifetimeCoroutine;

    private void OnEnable()
    {
        _lifetimeCoroutine = StartCoroutine(LifetimeCoroutine());
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ownerTag)) return;

        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null) GameManager.Instance.PlayerDied();
            ReturnToPool();
        }
        else if (other.CompareTag("Enemy") || other.CompareTag("Obstacle"))
        {
            ReturnToPool();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ownerTag)) return;

        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null) health.TakeDamage(damage);

        ReturnToPool();
    }

    private IEnumerator LifetimeCoroutine()
    {
        yield return new WaitForSeconds(lifetime);
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (ObjectPool.Instance != null)
        {
            ObjectPool.Instance.ReturnToPool(poolTag, gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        if (_lifetimeCoroutine != null) StopCoroutine(_lifetimeCoroutine);
    }
}
