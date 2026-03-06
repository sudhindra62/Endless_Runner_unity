
using UnityEngine;
using System;

public class DistanceTracker : MonoBehaviour
{
    public static DistanceTracker Instance { get; private set; }
    public static event Action<float> OnDistanceChanged;

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
            float distanceDelta = Vector3.Distance(playerMovement.transform.position, lastPosition);
            Distance += distanceDelta;
            lastPosition = playerMovement.transform.position;
            OnDistanceChanged?.Invoke(Distance);
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
