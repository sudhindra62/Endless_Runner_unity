
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Procedurally generates the level by spawning and placing level sections.
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    [Header("Level Sections")]
    [SerializeField] private List<GameObject> levelSections;
    [SerializeField] private GameObject initialSection;
    [SerializeField] private Transform playerTransform;

    [Header("Generation Settings")]
    [SerializeField] private int initialSectionsToSpawn = 3;
    [SerializeField] private float sectionLength = 50f; // Length of one section
    [SerializeField] private float spawnDistanceThreshold = 60f; // How far ahead to spawn the next section

    private List<GameObject> activeSections = new List<GameObject>();
    private float lastSpawnZ; // Z position of the last spawned section

    void Start()
    {
        // Spawn initial sections
        if (initialSection != null)
        {
            GameObject initial = Instantiate(initialSection, Vector3.zero, Quaternion.identity);
            activeSections.Add(initial);
            lastSpawnZ = 0f;
        }

        for (int i = 0; i < initialSectionsToSpawn; i++)
        {
            SpawnSection();
        }
    }

    void Update()
    {
        // Check if we need to spawn a new section
        if (playerTransform.position.z + spawnDistanceThreshold > lastSpawnZ)
        {
            SpawnSection();
            CleanUpOldSections();
        }
    }

    private void SpawnSection()
    {
        if (levelSections.Count == 0) return;

        int randomIndex = Random.Range(0, levelSections.Count);
        GameObject newSectionPrefab = levelSections[randomIndex];

        lastSpawnZ += sectionLength;
        Vector3 spawnPosition = new Vector3(0, 0, lastSpawnZ);

        GameObject newSection = Instantiate(newSectionPrefab, spawnPosition, Quaternion.identity);
        activeSections.Add(newSection);
    }

    private void CleanUpOldSections()
    {
        // Keep a buffer of sections behind the player
        float cleanupThreshold = playerTransform.position.z - (sectionLength * 2);

        for (int i = activeSections.Count - 1; i >= 0; i--)
        {
            if (activeSections[i].transform.position.z < cleanupThreshold)
            {
                GameObject sectionToDestroy = activeSections[i];
                activeSections.RemoveAt(i);
                Destroy(sectionToDestroy);
            }
        }
    }
}
