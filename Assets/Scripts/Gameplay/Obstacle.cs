
using UnityEngine;

/// <summary>
/// A simple marker script for objects that are considered obstacles.
/// The PlayerController will react to collisions with any object having this script.
/// </summary>
public class Obstacle : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // The main logic is on the PlayerController, but you could add
        // specific effects here, like playing a particle effect or sound.
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit an obstacle!");
        }
    }
}
