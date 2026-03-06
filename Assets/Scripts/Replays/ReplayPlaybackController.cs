
using UnityEngine;

/// <summary>
/// Controls the playback of a recorded replay. 
/// Manages the ghost character's position and actions based on ReplayData.
/// Provides controls for play, pause, and speed.
/// </summary>
public class ReplayPlaybackController : MonoBehaviour
{
    public bool IsPlaying { get; private set; }
    public float PlaybackSpeed { get; private set; } = 1f;

    private ReplayData replayData;
    private int currentFrameIndex;
    private float playbackTimer;
    
    [SerializeField] private GameObject ghostPrefab; // The visual representation of the player
    private Transform ghostTransform;

    public void StartPlayback(ReplayData data)
    {
        this.replayData = data;
        if (replayData == null || replayData.frames.Count == 0)
        {
            Debug.LogError("Cannot start playback: Replay data is invalid.");
            return;
        }

        if (ghostTransform == null)
        {
            ghostTransform = Instantiate(ghostPrefab, replayData.frames[0].position, replayData.frames[0].rotation).transform;
        }

        currentFrameIndex = 0;
        playbackTimer = 0f;
        IsPlaying = true;
    }

    private void Update()
    {
        if (!IsPlaying || replayData == null) return;

        playbackTimer += Time.deltaTime * PlaybackSpeed;

        // Advance frames until we catch up to the current playback time
        while (currentFrameIndex < replayData.frames.Count - 1 && replayData.frames[currentFrameIndex + 1].timestamp <= playbackTimer)
        {
            currentFrameIndex++;
        }

        // Interpolate between the current and next frame for smooth movement
        if (currentFrameIndex < replayData.frames.Count - 1)
        {
            ReplayFrame currentFrame = replayData.frames[currentFrameIndex];
            ReplayFrame nextFrame = replayData.frames[currentFrameIndex + 1];

            float t = (playbackTimer - currentFrame.timestamp) / (nextFrame.timestamp - currentFrame.timestamp);
            ghostTransform.position = Vector3.Lerp(currentFrame.position, nextFrame.position, t);
            ghostTransform.rotation = Quaternion.Slerp(currentFrame.rotation, nextFrame.rotation, t);
        }
        else
        {
            // We are at the last frame, snap to it
            ghostTransform.position = replayData.frames[currentFrameIndex].position;
            ghostTransform.rotation = replayData.frames[currentFrameIndex].rotation;
            IsPlaying = false; // End of replay
            Debug.Log("Replay finished.");
        }
    }

    public void Play()
    {
        IsPlaying = true;
    }

    public void Pause()
    {
        IsPlaying = false;
    }

    public void SetSpeed(float speed)
    {
        if (speed <= 0) return;
        PlaybackSpeed = speed;
    }
}
