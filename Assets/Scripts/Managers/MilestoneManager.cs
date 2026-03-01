
using System;
using UnityEngine;

public class MilestoneManager : MonoBehaviour
{
    public static MilestoneManager Instance { get; private set; }
    public static event Action<int> OnMilestoneReached;

    [SerializeField] private int milestoneInterval = 1000;

    private float distanceTraveled = 0f;
    private int nextMilestone = 0;
    private bool isTracking = false;

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

    private void Start()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
        nextMilestone = milestoneInterval;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void Update()
    {
        if (isTracking)
        {
            // Assuming PlayerMovement has a GetSpeed() method or similar
            // In a real project, this would be driven by the player's actual movement
            distanceTraveled += Time.deltaTime * 10f; // Placeholder speed

            if (distanceTraveled >= nextMilestone)
            {
                OnMilestoneReached?.Invoke(nextMilestone);
                nextMilestone += milestoneInterval;
            }
        }
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.Playing)
        {
            isTracking = true;
        }
        else if (newState == GameState.RunStart || newState == GameState.Menu)
        {
            isTracking = false;
            distanceTraveled = 0f;
            nextMilestone = milestoneInterval;
        }
        else
        {
            isTracking = false;
        }
    }
}
