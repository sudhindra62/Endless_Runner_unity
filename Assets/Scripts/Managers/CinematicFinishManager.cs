
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CinematicFinishManager : Singleton<CinematicFinishManager>
{
    [Header("Dependencies")]
    [SerializeField] private GameFlowController gameFlowController;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TimeControlManager timeControlManager;
    [SerializeField] private CameraShakeController cameraShakeController;
    [SerializeField] private AlmostWinManager almostWinManager;

    [Header("Slow-Motion Settings")]
    [SerializeField] private float slowMotionTimeScale = 0.2f;
    [SerializeField] private float slowMotionDuration = 1.0f;

    [Header("Replay Buffer Settings")]
    [SerializeField] private float replayBufferDuration = 3.0f;
    private List<Vector3> replayBuffer = new List<Vector3>();
    private bool isRecording = false;

    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void Update()
    {
        if (isRecording)
        {
            replayBuffer.Add(transform.position);
        }
    }

    public void StartRecording()
    {
        isRecording = true;
        StartCoroutine(ReplayBufferPruner());
    }

    public void StopRecording()
    {
        isRecording = false;
        replayBuffer.Clear();
    }

    private IEnumerator ReplayBufferPruner()
    {
        while (isRecording)
        {
            if (replayBuffer.Count > 0 && (Time.time - replayBuffer[0].z) > replayBufferDuration)
            {
                replayBuffer.RemoveAt(0);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void HandlePlayerDeath()
    {
        StartCoroutine(DeathCinematicCoroutine());
    }

    private IEnumerator DeathCinematicCoroutine()
    {
        timeControlManager.SetTimeScale(slowMotionTimeScale);
        cameraShakeController.ShakeCamera(0.5f, slowMotionDuration);
        // Add screen desaturation and camera zoom-in logic here

        yield return new WaitForSeconds(slowMotionDuration);

        timeControlManager.SetTimeScale(1.0f);

        string almostWinMessage = almostWinManager.GetAlmostWinMessage();
        if (!string.IsNullOrEmpty(almostWinMessage))
        {
            Debug.Log(almostWinMessage);
            // Display this message in the UI
        }

        gameFlowController.PlayerDied();
    }
}
