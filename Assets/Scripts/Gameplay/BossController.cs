
using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float chaseSpeed = 15f;
    [SerializeField] private float followDistance = 15f;

    [Header("Attack")]
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private Transform attackSpawnPoint;

    private Transform playerTransform;
    private Coroutine attackCoroutine;
    private ObjectPooler objectPooler;

    private void Awake()
    {
        playerTransform = FindObjectOfType<PlayerMovement>()?.transform;
        objectPooler = ObjectPooler.Instance;
    }

    private void OnEnable()
    {
        if (playerTransform != null)
        {
            StartCoroutine(ChasePlayer());
            attackCoroutine = StartCoroutine(PerformAttacks());
        }
    }

    private void OnDisable()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }

    private IEnumerator ChasePlayer()
    {
        while (true)
        {
            if (playerTransform != null)
            {
                Vector3 targetPosition = playerTransform.position - playerTransform.forward * followDistance;
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * chaseSpeed);
                transform.rotation = Quaternion.LookRotation(playerTransform.forward);
            }
            yield return null;
        }
    }

    private IEnumerator PerformAttacks()
    {
        yield return new WaitForSeconds(attackCooldown / 2); // Initial delay

        while (true)
        {
            if(Random.value > 0.5f)
            {
                PerformProjectileAttack();
            }
            else
            {
                PerformLaneBlockAttack();
            }

            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private void PerformProjectileAttack()
    {
        if (playerTransform != null)
        {
            Debug.Log("Boss is performing a projectile attack!");
            Vector3 attackPosition = attackSpawnPoint.position;
            Quaternion attackRotation = Quaternion.LookRotation(playerTransform.position - attackSpawnPoint.position);
            objectPooler.SpawnFromPool("BossProjectile", attackPosition, attackRotation);
        }
    }

    private void PerformLaneBlockAttack()
    {
        if (playerTransform == null) return;

        Debug.Log("Boss is performing a lane block attack!");

        int randomLane = Random.Range(-1, 2); // -1 for left, 0 for middle, 1 for right
        float laneOffset = randomLane * 3f; // Example lane offset, adjust as needed

        Vector3 barrierPosition = playerTransform.position + playerTransform.forward * 20f + new Vector3(laneOffset, 0.5f, 0);
        GameObject barrier = objectPooler.GetPooledObject("Barrier");
        
        if (barrier != null)
        {
            barrier.transform.position = barrierPosition;
            barrier.transform.rotation = playerTransform.rotation;
        }
    }

    public float GetDistanceFromPlayer()
    {
        if (playerTransform == null)
        {
            return float.MaxValue;
        }
        return Vector3.Distance(transform.position, playerTransform.position);
    }
}
