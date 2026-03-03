using UnityEngine;
using System.Collections.Generic;
using System;

public class TimeWarpManager : Singleton<TimeWarpManager>
{
    public static event Action OnTimeWarpUsed;

    [Header("Time Warp Settings")]
    [SerializeField] private float bufferDuration = 1f;
    [SerializeField] private float bufferInterval = 0.1f;

    private RunSessionData runSessionData;
    private PlayerMovement playerMovement;
    private CharacterController characterController;

    private readonly List<PlayerState> playerStateBuffer = new List<PlayerState>();
    private float lastSnapshotTime;
    private bool isWarping = false;

    private struct PlayerState
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public int Lane;
    }

    private void Start()
    {
        // Using a Service Locator pattern would be a more robust way to handle dependencies
        runSessionData = FindObjectOfType<RunSessionData>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        characterController = playerMovement?.GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        GameManager.OnRunStart += StartRecording;
        GameManager.OnRunEnd += StopRecording;
    }

    private void OnDisable()
    {
        GameManager.OnRunStart -= StartRecording;
        GameManager.OnRunEnd -= StopRecording;
    }

    private void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.Playing && !isWarping)
        {
            if (Time.time - lastSnapshotTime > bufferInterval)
            {
                RecordPlayerState();
                lastSnapshotTime = Time.time;
            }
        }
    }

    public void AttemptTimeWarp()
    {
        if (CanWarp())
        {
            PerformWarp();
        }
    }

    private bool CanWarp()
    {
        // SAFETY CHECKS:
        // 1. Ensure runSessionData is available.
        // 2. Check if the warp has been used this run.
        // 3. Ensure there are states to warp to.
        // 4. Prevent warping if already in the process of warping.
        // 5. Prevent warping if the game is not in the 'Playing' state (e.g., after death animation has started).
        return runSessionData != null && runSessionData.CanUseTimeWarp() && playerStateBuffer.Count > 0 && !isWarping && GameManager.Instance.CurrentState == GameState.Playing;
    }

    private void PerformWarp()
    {
        isWarping = true;

        // Get the oldest state in the buffer
        PlayerState warpState = playerStateBuffer[0];

        // Disable the CharacterController to directly set the position, preventing physics conflicts.
        if (characterController != null) characterController.enabled = false;

        // Restore Player State
        playerMovement.transform.position = warpState.Position;
        if (playerMovement.TryGetComponent<Rigidbody>(out var rb)) // Check for a Rigidbody to restore velocity
        {
            rb.velocity = warpState.Velocity;
        }
        playerMovement.ChangeLane(warpState.Lane - playerMovement.currentLane); // Restore lane by calculating the difference

        // Re-enable the CharacterController.
        if (characterController != null) characterController.enabled = true;

        // Consume the warp and notify other systems.
        runSessionData.UseTimeWarp();
        OnTimeWarpUsed?.Invoke();

        // Clear the buffer to prevent reuse.
        playerStateBuffer.Clear();

        Debug.Log("Time Warp successful!");

        isWarping = false;
    }

    private void RecordPlayerState()
    {
        if (playerMovement == null || characterController == null) return;

        playerStateBuffer.Add(new PlayerState
        {
            Position = playerMovement.transform.position,
            Velocity = characterController.velocity,
            Lane = playerMovement.currentLane
        });

        // Trim the buffer to maintain the 1-second window.
        float bufferTime = playerStateBuffer.Count * bufferInterval;
        if (bufferTime > bufferDuration)
        {
            playerStateBuffer.RemoveAt(0);
        }
    }

    private void StartRecording()
    {
        playerStateBuffer.Clear();
        isWarping = false;
    }

    private void StopRecording()
    {
        playerStateBuffer.Clear();
    }
}
