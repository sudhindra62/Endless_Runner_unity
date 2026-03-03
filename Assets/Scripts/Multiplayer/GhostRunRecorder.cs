using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GhostRunRecorder : MonoBehaviour
{
    [SerializeField] private Transform targetToRecord;
    [SerializeField] private float recordInterval = 0.1f;

    private List<Vector3> recordedPositions = new List<Vector3>();
    private bool isRecording = false;
    private float recordTimer = 0f;

    public void StartRecording()
    {
        if (targetToRecord == null) 
        {
            Debug.LogError("No target assigned to GhostRunRecorder.");
            return;
        }
        recordedPositions.Clear();
        isRecording = true;
        Debug.Log("Ghost recording started.");
    }

    public byte[] StopRecordingAndGetData()
    {
        isRecording = false;
        byte[] data = SerializePositions();
        Debug.Log($"Ghost recording stopped. Data size: {data.Length} bytes");
        return data;
    }

    private void Update()
    {
        if (isRecording)
        {
            recordTimer += Time.deltaTime;
            if (recordTimer >= recordInterval)
            {
                recordedPositions.Add(targetToRecord.position);
                recordTimer = 0f;
            }
        }
    }

    private byte[] SerializePositions()
    {
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                // DATA RULE: Include final score (placeholder)
                writer.Write(ScoreManager.Instance.Score);

                foreach (Vector3 pos in recordedPositions)
                {
                    writer.Write(pos.x);
                    writer.Write(pos.y);
                    writer.Write(pos.z);
                }
                // DATA RULE: Compressed (placeholder - real compression would be used here)
                // DATA RULE: Checksum (placeholder - a checksum algorithm would be used here)
                return stream.ToArray();
            }
        }
    }
}
