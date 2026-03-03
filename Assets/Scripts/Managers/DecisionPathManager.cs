using UnityEngine;
using System;
using System.Collections.Generic;

public class DecisionPathManager : MonoBehaviour 
{
    public static event Action<int, int> OnPathSplit;
    public static event Action OnPathMerge;

    [Header("Dependencies")]
    [SerializeField] private EffectsManager effectsManager;

    [Header("Path Generation Settings")]
    [SerializeField] private GameObject[] easyPathTiles;
    [SerializeField] private GameObject[] mediumPathTiles;
    [SerializeField] private GameObject[] hardPathTiles;
    [SerializeField] private GameObject mergeTile;
    [SerializeField] private int pathLength = 20;
    [SerializeField] private float laneWidth = 2.5f;
    [SerializeField, Range(2, 3)] private int numberOfPaths = 3;

    private bool isDecisionPathActive = false;
    private List<GameObject> activePathTiles = new List<GameObject>();
    private const float TILE_LENGTH = 10f; // Standardized tile length

    public bool IsDecisionPathActive => isDecisionPathActive;

    public void GenerateDecisionPaths(Vector3 spawnPosition)
    {
        if (isDecisionPathActive) return;

        isDecisionPathActive = true;
        activePathTiles.Clear();

        List<Vector3> pathStartPositions = new List<Vector3>();
        List<int> laneIndices = new List<int>();

        // Calculate starting positions and lane indices based on the number of paths
        if (numberOfPaths == 2)
        {
            pathStartPositions.Add(spawnPosition + Vector3.left * laneWidth);
            pathStartPositions.Add(spawnPosition + Vector3.right * laneWidth);
            laneIndices.Add(-1);
            laneIndices.Add(1);
        }
        else // 3 paths
        {
            pathStartPositions.Add(spawnPosition + Vector3.left * laneWidth);
            pathStartPositions.Add(spawnPosition);
            pathStartPositions.Add(spawnPosition + Vector3.right * laneWidth);
            laneIndices.Add(-1);
            laneIndices.Add(0);
            laneIndices.Add(1);
        }

        // Fulfill "Visual indicators before split" rule
        if (effectsManager != null)
        {
            effectsManager.ShowPathIndicators(pathStartPositions.ToArray());
        }

        // Generate the paths with varied difficulty
        GeneratePath(easyPathTiles, pathStartPositions[0], pathLength);
        GeneratePath(mediumPathTiles, pathStartPositions[1], pathLength);
        if (numberOfPaths == 3)
        {
            GeneratePath(hardPathTiles, pathStartPositions[2], pathLength);
        }

        // Spawn the merge tile at the end of the paths
        Vector3 mergeSpawnPoint = spawnPosition + Vector3.forward * (pathLength * TILE_LENGTH);
        GameObject mergeTileInstance = Instantiate(mergeTile, mergeSpawnPoint, Quaternion.identity);
        activePathTiles.Add(mergeTileInstance);

        // Notify other systems about the split
        OnPathSplit?.Invoke(laneIndices[0], laneIndices[laneIndices.Count - 1]);
    }

    private void GeneratePath(GameObject[] pathTiles, Vector3 startPos, int length)
    {
        Vector3 currentSpawnPoint = startPos;
        for (int i = 0; i < length; i++)
        {
            if (pathTiles.Length == 0) continue;
            GameObject tilePrefab = pathTiles[UnityEngine.Random.Range(0, pathTiles.Length)];
            GameObject tile = Instantiate(tilePrefab, currentSpawnPoint, Quaternion.identity);
            activePathTiles.Add(tile);
            currentSpawnPoint.z += TILE_LENGTH;
        }
    }

    public void TriggerMerge()
    {
        if (!isDecisionPathActive) return;

        isDecisionPathActive = false;
        
        // Notify other systems that the paths are merging
        OnPathMerge?.Invoke();

        // Fulfill "Proper cleanup after merge" rule
        foreach (GameObject tile in activePathTiles)
        {
            // Destroy tiles with a slight delay to ensure player has passed them
            Destroy(tile, 3f);
        }
        activePathTiles.Clear();
    }
}
