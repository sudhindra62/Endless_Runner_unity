
using UnityEngine;

/// <summary>
/// Records player actions and game state during a run to create a ReplayData file.
/// Attaches to the player character.
/// </summary>
public class ReplayRecorder : MonoBehaviour
{
    public bool IsRecording { get; private set; }
    private ReplayData currentReplay;
    private float recordTimer;
    private float runTimer;

    private const float RECORD_INTERVAL = 0.2f;
    private const float MAX_REPLAY_SECONDS = 600f; // 10 minutes

    // References to player components
    private PlayerController playerController;

    private void Start()
    {
        // These would be linked on instantiation
        // playerController = GetComponent<PlayerController>(); 
        
        GameFlowController.OnRunStarted += StartRecording;
        GameFlowController.OnRunEnded += StopRecording;
    }

    private void OnDestroy()
    {
        GameFlowController.OnRunStarted -= StartRecording;
        GameFlowController.OnRunEnded -= StopRecording;
    }

    private void Update()
    {
        if (IsRecording)
        {
            runTimer += Time.deltaTime;
            recordTimer += Time.deltaTime;

            if (recordTimer >= RECORD_INTERVAL)
            {
                recordTimer -= RECORD_INTERVAL;
                RecordFrame();
            }

            if (runTimer >= MAX_REPLAY_SECONDS)
            {
                StopRecording();
            }
        }
    }

    public void StartRecording()
    {
        currentReplay = new ReplayData();
        IsRecording = true;
        runTimer = 0f;
        Debug.Log("Replay recording started.");
    }

    public void StopRecording()
    {
        if (!IsRecording) return;

        IsRecording = false;
        // currentReplay.score = ScoreManager.Instance.GetCurrentScore();
        // currentReplay.distance = ScoreManager.Instance.GetCurrentDistance();
        
        ReplayManager.Instance.SaveReplay(currentReplay);
        Debug.Log("Replay recording stopped and saved.");
    }

    private void RecordFrame()
    {
        // In a real implementation, we'd get the action from the PlayerController
        PlayerAction action = PlayerAction.None; // GetLastAction();

        ReplayFrame frame = new ReplayFrame
        {
            timestamp = runTimer,
            position = transform.position,
            rotation = transform.rotation,
            action = action
        };

        currentReplay.frames.Add(frame);
    }
    
    // Example of how an action would be captured
    public void OnPlayerAction(PlayerAction action) 
    {
        if(IsRecording) 
        {
            // This is an alternative to polling, actions are pushed from the controller
            // RecordFrameWithAction(action);
        }
    }
}
