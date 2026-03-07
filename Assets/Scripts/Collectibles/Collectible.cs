
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    [Header("Collectible")]
    [SerializeField] private int scoreValue = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollect();
        }
    }

    protected virtual void OnCollect()
    {
        // Add score
        ScoreManager.Instance.AddScore(scoreValue);

        // Deactivate the collectible
        gameObject.SetActive(false);
    }
}
