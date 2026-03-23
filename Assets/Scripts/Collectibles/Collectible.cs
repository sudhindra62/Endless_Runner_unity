
using UnityEngine;

public abstract class Collectible : MonoBehaviour
{
    [Header("Collectible")]
    [SerializeField] private int scoreValue = 10;
    protected string poolTag;

    protected virtual void Start() { }

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
