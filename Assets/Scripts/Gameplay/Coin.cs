using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue = 10;
    
    private bool isMoving = false;
    private Transform target;
    private float moveSpeed;

    private void Start()
    {
        if (ThemeManager.Instance != null)
        {
            // Optional: apply theme specific logic
        }
    }

    void Update()
    {
        if (isMoving && target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        }
    }

    public void MoveTowards(Transform target, float speed)
    {
        this.target = target;
        this.moveSpeed = speed;
        this.isMoving = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        else if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }

        if (ObjectPooler.Instance != null)
        {
            ObjectPooler.Instance.ReturnToPool("Coin", gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        isMoving = false;
    }
}
