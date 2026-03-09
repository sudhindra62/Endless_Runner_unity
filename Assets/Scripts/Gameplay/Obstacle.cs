
using UnityEngine;

/// <summary>
/// Defines the behavior of an obstacle that the player must avoid.
/// Now includes logic for being destroyed by a shielded player.
/// Fortified by Supreme Guardian Architect v12.
/// </summary>
public class Obstacle : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] private bool isDestructible = true;
    [SerializeField] private GameObject destructionEffect;

    private void OnCollisionEnter(Collision collision)
    {
        // The PlayerController now handles all collision logic.
        // This script primarily serves as a tag and data container for obstacles.
        // Additional logic, like animations or unique obstacle behaviors, would go here.
    }

    /// <summary>
    /// Called by the PlayerController when a shield is active during a collision.
    /// </summary>
    public void Shatter()
    {
        if (!isDestructible) return;

        Debug.Log("Guardian Architect Log: Obstacle shattered by shield!");

        if (destructionEffect != null)
        {
            Instantiate(destructionEffect, transform.position, Quaternion.identity);
        }
        
        // Disable or pool the object
        gameObject.SetActive(false);
    }
}
