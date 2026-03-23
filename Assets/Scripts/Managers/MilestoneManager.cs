
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
    public float TotalDistanceTraveled => distanceTraveled;
    public int TotalCoinsCollected { get; private set; }
    public int TotalShieldsUsed { get; private set; }
    public int TotalJumps { get; private set; }
    public int TotalSlides { get; private set; }

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
        else if (newState == GameState.Starting || newState == GameState.MainMenu)
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

    public void CheckMilestones(RunSessionData session)
    {
        if (session == null) return;
        // Delegate to the inner tracking logic
        if (session.TotalScore >= nextMilestone)
        {
            OnMilestoneReached?.Invoke(nextMilestone);
            nextMilestone += milestoneInterval;
        }
    }

    public void CheckMilestones(MilestoneType type, float value)
    {
        Debug.Log($"[Milestone] Checking {type} with value {value}");
        switch (type)
        {
            case MilestoneType.Distance:
                distanceTraveled = value;
                break;
            case MilestoneType.CoinsCollected:
                TotalCoinsCollected = Mathf.RoundToInt(value);
                break;
            case MilestoneType.ShieldsUsed:
                TotalShieldsUsed = Mathf.RoundToInt(value);
                break;
            case MilestoneType.Jumps:
                TotalJumps = Mathf.RoundToInt(value);
                break;
            case MilestoneType.Slides:
                TotalSlides = Mathf.RoundToInt(value);
                break;
        }
        // Add logic to check specific milestones based on type and value
    }

    public void CheckMilestones(MilestoneType type, int value)
    {
        CheckMilestones(type, (float)value);
    }
}
