
using UnityEngine;
using System.Collections.Generic;

public class GhostRunRecorder : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float recordingInterval = 0.1f;

    private List<GhostDataPoint> ghostDataPoints = new List<GhostDataPoint>();
    private float lastRecordingTime;
    private bool isRecording;

    private void OnEnable()
    {
        GameManager.OnRunStart += StartRecording;
        PlayerController.OnPlayerDeath += StopRecording;
    }

    private void OnDisable()
    {
        GameManager.OnRunStart -= StartRecording;
        PlayerController.OnPlayerDeath -= StopRecording;
    }

    private void StartRecording()
    {
        ghostDataPoints.Clear();
        isRecording = true;
        lastRecordingTime = Time.time;
    }

    private void StopRecording()
    {
        isRecording = false;
        GhostRunManager.Instance.SaveNewBestRun(ghostDataPoints);
    }

    private void Update()
    {
        if (isRecording && Time.time - lastRecordingTime >= recordingInterval)
        {
            RecordDataPoint();
            lastRecordingTime = Time.time;
        }
    }

    private void RecordDataPoint()
    {
        ghostDataPoints.Add(new GhostDataPoint
        {
            timestamp = Time.timeSinceLevelLoad,
            position = playerController.transform.position,
            isJumping = playerController.IsJumping(),
            isSliding = playerController.IsSliding()
        });
    }
}

[System.Serializable]
public struct GhostDataPoint
{
    public float timestamp;
    public Vector3 position;
    public bool isJumping;
    public bool isSliding;
}
