
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public static DistanceTracker Instance { get; private set; }

    public float Distance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // This is a simple implementation that assumes the player is always moving forward.
        // In a real implementation, you would want to use a more accurate method of tracking distance.
        Distance += Time.deltaTime * 10f;
    }

    public void ResetDistance()
    {
        Distance = 0f;
    }
}
