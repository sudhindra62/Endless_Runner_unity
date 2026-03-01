
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    public static DistanceTracker Instance { get; private set; }

    public float Distance { get; private set; }

    private PlayerMovement playerMovement;
    private Vector3 lastPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            lastPosition = playerMovement.transform.position;
        }
    }

    private void Update()
    {
        if (playerMovement != null)
        {
            Distance += Vector3.Distance(playerMovement.transform.position, lastPosition);
            lastPosition = playerMovement.transform.position;
        }
    }

    public void ResetDistance()
    {
        Distance = 0;
        if (playerMovement != null)
        {
            lastPosition = playerMovement.transform.position;
        }
    }
}
