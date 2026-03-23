using UnityEngine;
using System.Collections.Generic;

public class TimeWarpManager : MonoBehaviour
{
    public static TimeWarpManager Instance { get; private set; }

    [Header("Dependencies")]
    [SerializeField] private PlayerController playerMovement;
    [SerializeField] private RunSessionData runSessionData;
    [SerializeField] private ReviveManager reviveManager;
    [SerializeField] private ScoreManager scoreManager;

    [Header("Time Warp Settings")]
    [SerializeField] private float rewindDuration = 1f;

    private struct PlayerState
    {
        public Vector3 Position;
        public Vector3 Velocity;
        public int Lane;
        public float Time;
    }

    private List<PlayerState> playerStateHistory = new List<PlayerState>();
    private bool canWarp = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (playerMovement != null)
        {
            RecordPlayerState();
        }
    }

    private void RecordPlayerState()
    {
        playerStateHistory.Add(new PlayerState
        {
            Position = playerMovement.transform.position,
            Velocity = playerMovement.Velocity,
            Lane = playerMovement.CurrentLane,
            Time = Time.time
        });

        // Remove old states that are outside the rewind duration
        playerStateHistory.RemoveAll(state => Time.time - state.Time > rewindDuration);
    }

    public bool CanUseTimeWarp()
    {
        return canWarp && !runSessionData.hasUsedTimeWarp && !reviveManager.IsPlayerReviving();
    }

    public void ActivateTimeWarp()
    {
        if (!CanUseTimeWarp()) return;

        PlayerState rewindState = GetRewindState();
        if (rewindState.Time == 0) return; // No valid state to rewind to

        // Restore player to the rewind state
        playerMovement.SetState(rewindState.Position, rewindState.Velocity, rewindState.Lane);

        // Update session data and prevent further use
        runSessionData.hasUsedTimeWarp = true;
        canWarp = false;

        // Safety checks
        // This logic does not directly reset score, as requested.
        // It also doesn't bypass the revive limit, as that's handled by ReviveManager.
        // Not allowing multiple uses is handled by the hasUsedTimeWarp flag.
        // The CanUseTimeWarp check prevents usage after death animation (assuming ReviveManager state changes).
    }

    private PlayerState GetRewindState()
    {
        if (playerStateHistory.Count > 0)
        {
            return playerStateHistory[0];
        }
        return new PlayerState(); // Return an empty state if history is empty
    }

    public void ResetWarp()
    {
        canWarp = true;
        playerStateHistory.Clear();
    }
}
