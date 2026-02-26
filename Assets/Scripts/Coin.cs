using UnityEngine;

/// <summary>
/// Unified Coin Script
/// Supports:
/// - Trigger collection
/// - Controller collision collection
/// - Object pooling
/// - Safe single collection
/// </summary>
public class Coin : MonoBehaviour
{
    [Tooltip("Score value this coin provides.")]
    public int value = 1;

    private bool hasBeenCollected = false;

    private void OnEnable()
    {
        hasBeenCollected = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

 public void Collect()
{
    if (hasBeenCollected) return;

    hasBeenCollected = true;

    if (ScoreManager.Instance != null)
    {
        ScoreManager.Instance.AddScore(value);
    }

    gameObject.SetActive(false);
}

}
