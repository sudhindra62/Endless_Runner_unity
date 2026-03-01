using UnityEngine;
using System.Collections;

/// <summary>
/// This component attracts GameObjects with the "Coin" tag towards the transform it is attached to.
/// It uses a SphereCollider as a trigger to detect nearby coins and can have its radius and attraction force adjusted.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public class Magnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    [Tooltip("The speed at which coins are pulled towards the magnet.")]
    [SerializeField] private float attractionForce = 15f;
    [Tooltip("The base radius of the magnet's attraction field.")]
    [SerializeField] private float baseRadius = 5f;

    private SphereCollider magnetTrigger;
    private Coroutine attractionCoroutine;

    private void Awake()
    {
        magnetTrigger = GetComponent<SphereCollider>();
        magnetTrigger.isTrigger = true;
        magnetTrigger.radius = baseRadius;
    }

    private void OnEnable()
    {
        // Start the coroutine to handle coin attraction.
        // This is more performant than checking for coins in OnTriggerStay every frame.
        if (attractionCoroutine != null)
        {
            StopCoroutine(attractionCoroutine);
        }
        attractionCoroutine = StartCoroutine(AttractCoinsRoutine());
    }

    private void OnDisable()
    {
        // Stop the coroutine when the magnet is disabled.
        if (attractionCoroutine != null)
        {
            StopCoroutine(attractionCoroutine);
            attractionCoroutine = null;
        }
    }

    /// <summary>
    /// Sets the radius of the magnet's trigger collider. Can be used for power-up fusions.
    /// </summary>
    /// <param name="newRadius">The new radius of the attraction field.</param>
    public void SetRadius(float newRadius)
    {
        magnetTrigger.radius = Mathf.Max(0, newRadius);
    }

    /// <summary>
    /// Resets the magnet's radius to its original base value.
    /// </summary>
    public void ResetRadius()
    {
        magnetTrigger.radius = baseRadius;
    }

    private IEnumerator AttractCoinsRoutine()
    {
        var waitForFixedUpdate = new WaitForFixedUpdate();
        while (true)
        { 
            // Check for coins in the overlap sphere. This is generally better for performance than OnTriggerStay.
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, magnetTrigger.radius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Coin"))
                {
                    // Move the coin towards the magnet
                    hitCollider.transform.position = Vector3.MoveTowards(hitCollider.transform.position, transform.position, attractionForce * Time.fixedDeltaTime);
                }
            }
            yield return waitForFixedUpdate;
        }
    }
}
