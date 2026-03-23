using UnityEngine;

/// <summary>
/// Represents a collectible power-up item within the game world.
/// Global scope.
/// </summary>
[RequireComponent(typeof(Collider))]
public class PowerUpCollectible : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private PowerUpDefinition powerUpDefinition;

    [Header("Visual Effects")]
    [SerializeField] private GameObject collectionEffect;
    [SerializeField] private Transform rotatingVisual;
    [SerializeField] private float rotationSpeed = 100f;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void Update()
    {
        if (rotatingVisual != null)
        {
            rotatingVisual.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) Collect();
    }

    private void Collect()
    {
        if (powerUpDefinition == null) return;

        if (PowerUpManager.Instance != null)
        {
            PowerUpManager.Instance.ActivatePowerUp(powerUpDefinition);
        }

        if (collectionEffect != null)
        {
            Instantiate(collectionEffect, transform.position, Quaternion.identity);
        }
        
        gameObject.SetActive(false);
    }
}
