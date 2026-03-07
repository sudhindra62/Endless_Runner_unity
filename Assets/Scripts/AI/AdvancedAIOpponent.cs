
using UnityEngine;

[RequireComponent(typeof(WaypointPatrol))]
[RequireComponent(typeof(Health))]
public class AdvancedAIOpponent : MonoBehaviour
{
    public enum AIState { Patrol, Chase, Attack }

    public float speed = 5.0f;
    public float obstacleAvoidanceDistance = 2.0f;
    public LayerMask obstacleLayer;

    public float playerDetectionRadius = 10f;
    public float attackRange = 2f;
    public LayerMask playerLayer;

    public float waypointArrivalDistance = 0.5f;

    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 1f;
    private float nextFireTime = 0f;

    public int pointsOnDeath = 100;

    private AIState currentState;
    private Transform detectedPlayer;
    private WaypointPatrol waypointPatrol;

    void Start()
    {
        gameObject.tag = "Enemy";
        currentState = AIState.Patrol;
        waypointPatrol = GetComponent<WaypointPatrol>();
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Chase:
                Chase();
                break;
            case AIState.Attack:
                Attack();
                break;
        }
    }

    void Patrol()
    {
        DetectPlayer();
        if (detectedPlayer != null)
        {
            currentState = AIState.Chase;
            return;
        }

        Transform currentWaypoint = waypointPatrol.GetCurrentWaypoint();
        if (currentWaypoint != null)
        {
            if (Vector3.Distance(transform.position, currentWaypoint.position) < waypointArrivalDistance)
            {
                waypointPatrol.GoToNextWaypoint();
            }
            MoveTowardsTarget(currentWaypoint);
        }
    }

    void Chase()
    {
        if (detectedPlayer == null)
        {
            currentState = AIState.Patrol;
            return;
        }

        if (Vector3.Distance(transform.position, detectedPlayer.position) <= attackRange)
        {
            currentState = AIState.Attack;
            return;
        }

        MoveTowardsTarget(detectedPlayer);
    }

    void Attack()
    {
        if (detectedPlayer == null || Vector3.Distance(transform.position, detectedPlayer.position) > attackRange)
        {
            currentState = AIState.Chase;
            return;
        }

        transform.LookAt(detectedPlayer);

        if (Time.time >= nextFireTime)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.GetComponent<Projectile>().ownerTag = "Enemy";
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void DetectPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, playerDetectionRadius, playerLayer);
        if (hitColliders.Length > 0)
        {
            detectedPlayer = hitColliders[0].transform;
        }
        else
        {
            detectedPlayer = null;
        }
    }

    void MoveTowardsTarget(Transform target)
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, obstacleAvoidanceDistance, obstacleLayer))
        {
            Vector3 avoidanceDirection = Vector3.Cross(Vector3.up, hit.normal);
            directionToTarget += avoidanceDirection;
        }

        transform.position = Vector3.MoveTowards(transform.position, transform.position + directionToTarget, speed * Time.deltaTime);

        if (directionToTarget != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(directionToTarget);
        }
    }

    void OnDestroy()
    {
        if (ScoringManager.instance != null)
        {
            ScoringManager.instance.AddScore(pointsOnDeath);
        }
    }
}
