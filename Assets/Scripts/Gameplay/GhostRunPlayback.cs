
using UnityEngine;
using System.Collections.Generic;

public class GhostRunPlayback : MonoBehaviour
{
    [SerializeField] private GameObject ghostPrefab; // Assign a ghost character prefab in the Inspector
    [SerializeField] private float transparency = 0.5f;

    private GameObject ghostInstance;
    private List<GhostDataPoint> ghostData;
    private int currentDataIndex;
    private bool isPlaying;

    public void LoadGhostRun(List<GhostDataPoint> data)
    {
        ghostData = data;
        currentDataIndex = 0;
        isPlaying = true;

        if (ghostInstance == null)
        {
            ghostInstance = Instantiate(ghostPrefab, transform);
            // Make the ghost transparent
            var renderer = ghostInstance.GetComponentInChildren<Renderer>();
            if (renderer != null)
            {
                Color color = renderer.material.color;
                color.a = transparency;
                renderer.material.color = color;
            }
        }
    }

    private void Update()
    {
        if (!isPlaying || ghostData == null || currentDataIndex >= ghostData.Count)
        {
            return;
        }

        // Find the correct data point based on the current time
        while (currentDataIndex < ghostData.Count && Time.timeSinceLevelLoad >= ghostData[currentDataIndex].timestamp)
        {
            ghostInstance.transform.position = ghostData[currentDataIndex].position;
            // We can also add animations for jumping and sliding here if needed
            currentDataIndex++;
        }
    }

    public void StopPlayback()
    {
        isPlaying = false;
        if (ghostInstance != null)
        {
            Destroy(ghostInstance);
        }
    }
}
