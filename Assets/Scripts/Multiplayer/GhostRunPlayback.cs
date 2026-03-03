using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GhostRunPlayback : MonoBehaviour
{
    [SerializeField] private float playbackSpeed = 1f;
    
    private List<Vector3> ghostPositions = new List<Vector3>();
    private int currentPositionIndex = 0;
    private bool isPlaying = false;
    private float playbackTimer = 0f;
    private float recordInterval = 0.1f; // Must match the interval from the recorder

    public void StartPlayback(byte[] data)
    {
        DeserializePositions(data);
        if (ghostPositions.Count > 0)
        {
            isPlaying = true;
            // RACE RULE: Purely visual - create a simple representation
            GameObject ghostVisual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            ghostVisual.transform.SetParent(this.transform);
            ghostVisual.GetComponent<Collider>().enabled = false; // RACE RULE: No collision

            // UI: Ghost transparency
            var renderer = ghostVisual.GetComponent<Renderer>();
            Color ghostColor = Color.blue;
            ghostColor.a = 0.5f;
            renderer.material.color = ghostColor;

            Debug.Log("Ghost playback started.");
        }
    }

    private void Update()
    {
        if (isPlaying)
        {
            playbackTimer += Time.deltaTime * playbackSpeed;
            if (playbackTimer >= recordInterval)
            {
                currentPositionIndex++;
                if (currentPositionIndex >= ghostPositions.Count)
                {
                    StopPlayback();
                    return;
                }
                playbackTimer = 0f;
            }
            // Lerp for smooth movement between recorded points
            float lerpFactor = playbackTimer / recordInterval;
            transform.position = Vector3.Lerp(ghostPositions[currentPositionIndex], ghostPositions[currentPositionIndex + 1 < ghostPositions.Count ? currentPositionIndex + 1 : currentPositionIndex], lerpFactor);
        }
    }

    private void StopPlayback()
    {
        isPlaying = false;
        Debug.Log("Ghost playback finished.");
        // LONG SESSION SAFETY: Ensure no duplicate ghost spawn by destroying self
        Destroy(gameObject);
    }

    private void DeserializePositions(byte[] data)
    {
        using (MemoryStream stream = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                // DATA RULE: Read final score (placeholder for validation)
                int finalScore = reader.ReadInt32();

                ghostPositions.Clear();
                while (stream.Position < stream.Length)
                {
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    float z = reader.ReadSingle();
                    ghostPositions.Add(new Vector3(x, y, z));
                }
            }
        }
    }
}
