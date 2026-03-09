
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Geometry")]
    [SerializeField] private GameObject levelChunkPrefab;
    [SerializeField] private int chunkLength = 20;
    [SerializeField] private int initialChunks = 5;
    [SerializeField] private int chunksToMaintain = 3;

    [Header("Generation Parameters")]
    [SerializeField] private ProceduralPatternGenerator patternGenerator;
    [SerializeField] private float generationInterval = 5.0f; 

    private Queue<GameObject> _activeChunks = new Queue<GameObject>();
    private Vector3 _nextChunkPosition;
    private float _timeSinceLastGeneration = 0f;

    private void Start()
    {
        InitializeLevel();
        // Additional setup if required
    }

    private void Update()
    {
        _timeSinceLastGeneration += Time.deltaTime;
        if (_timeSinceLastGeneration >= generationInterval)
        { 
            GenerateAndPlaceChunk();
            _timeSinceLastGeneration = 0f;
        }

        // Optional: Implement a trigger-based generation system instead of time-based.
    }

    private void InitializeLevel()
    {
        _nextChunkPosition = transform.position;
        for (int i = 0; i < initialChunks; i++)
        {
            GenerateAndPlaceChunk();
        }
    }

    private void GenerateAndPlaceChunk()
    {
        int[] pattern = patternGenerator.GeneratePattern(chunkLength);
        GameObject newChunk = Instantiate(levelChunkPrefab, _nextChunkPosition, Quaternion.identity, transform);
        _nextChunkPosition.z += chunkLength;

        // The LevelChunk component will now be responsible for populating itself.
        LevelChunk chunkComponent = newChunk.GetComponent<LevelChunk>();
        if (chunkComponent != null)
        { 
            chunkComponent.Initialize(pattern);
        }

        _activeChunks.Enqueue(newChunk);

        if (_activeChunks.Count > chunksToMaintain)
        {
            Destroy(_activeChunks.Dequeue());
        }
    }
}
