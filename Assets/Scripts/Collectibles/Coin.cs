using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    [Tooltip("Score value this coin provides.")]
    public int value = 1;

    private bool hasBeenCollected = false;
    private bool isAttracted = false;

    private void OnEnable()
    {
        hasBeenCollected = false;
        isAttracted = false;
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

        if (RunStatsManager.Instance != null)
        {
            RunStatsManager.Instance.AddCoin();
        }

        MilestoneManager.Instance?.AddCoins(value);

        gameObject.SetActive(false);
    }

    public bool IsAttracted()
    {
        return isAttracted;
    }

    public void Attract(Transform target, float speed)
    {
        if (isAttracted) return;

        isAttracted = true;
        StartCoroutine(AttractionCoroutine(target, speed));
    }

    private IEnumerator AttractionCoroutine(Transform target, float speed)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
            yield return null;
        }

        // Once the coin is close enough, collect it.
        Collect();
    }
}
