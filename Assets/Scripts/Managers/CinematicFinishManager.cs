using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CinematicFinishManager : MonoBehaviour
{
    public static CinematicFinishManager Instance { get; private set; }

    [Header("Slow Motion Settings")]
    [SerializeField] private float slowMotionTimeScale = 0.2f;
    [SerializeField] private float slowMotionDuration = 0.7f;

    [Header("Replay Settings")]
    [SerializeField] private int replayBufferSize = 180; // 3 seconds at 60fps
    private List<Vector3> replayBuffer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        replayBuffer = new List<Vector3>(replayBufferSize);
    }

    public void OnPlayerDeath()
    {
        StartCoroutine(DeathSequence());
    }

    public void RecordPosition(Vector3 position)
    {
        if (replayBuffer.Count >= replayBufferSize)
        {
            replayBuffer.RemoveAt(0);
        }
        replayBuffer.Add(position);
    }

    public void ClearReplayBuffer()
    {
        replayBuffer.Clear();
    }

    private IEnumerator DeathSequence()
    {
        yield return StartCoroutine(SlowMotionEffect());

        if (AlmostWinManager.Instance != null)
        {
            AlmostWinManager.Instance.CheckAlmostWinConditions();
        }

        PlayReplay();
    }

    private IEnumerator SlowMotionEffect()
    {
        TimeControlManager.Instance.SetTimeScale(slowMotionTimeScale);
        yield return new WaitForSecondsRealtime(slowMotionDuration);

        if (ReviveManager.Instance != null && ReviveManager.Instance.IsReviveActive())
        {
            yield break;
        }

        TimeControlManager.Instance.SetTimeScale(1f);
    }

    private void PlayReplay()
    {
        // In a full implementation, this would trigger a replay system.
        // For now, we will just log the contents of the buffer.
        Debug.Log("--- REPLAY BUFFER ---");
        foreach (Vector3 position in replayBuffer)
        {
            Debug.Log(position);
        }
        Debug.Log("--- END REPLAY ---");
    }
}
