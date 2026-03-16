using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Chunk[] chunkPrefabs;
    public Chunk firstChunk;

    private void Start()
    {
        // Instantiate the first chunk
        Chunk lastChunk = Instantiate(firstChunk, transform.position, transform.rotation);

        // Spawn a few chunks to start
        for (int i = 0; i < 5; i++)
        {
            lastChunk = SpawnChunk(lastChunk);
        }
    }

    public Chunk SpawnChunk(Chunk previousChunk)
    {
        Chunk newChunk = Instantiate(GetRandomChunk());
        newChunk.transform.position = previousChunk.end.position - (newChunk.begin.position - newChunk.transform.position);
        return newChunk;
    }

    private Chunk GetRandomChunk()
    {
        return chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
    }
}
