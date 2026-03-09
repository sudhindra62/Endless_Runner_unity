
using UnityEngine;

/// <summary>
/// Manages the procedural pattern generation seed to ensure deterministic and replicable runs.
/// Inherits from Singleton to provide a globally accessible, persistent instance.
/// </summary>
public class PatternSeedManager : Singleton<PatternSeedManager>
{
    public int CurrentSeed { get; private set; }
    private System.Random _masterRandomGenerator;

    protected override void Awake()
    {
        base.Awake(); // This handles the singleton instance and DontDestroyOnLoad.
        // Initialize with a random seed by default.
        // Game-specific managers can set a deterministic seed later if needed.
        if (_masterRandomGenerator == null)
        { 
            SetNewRandomSeed();
        }
    }

    /// <summary>
    /// Sets a specific seed to begin a deterministic run.
    /// </summary>
    /// <param name="seed">The integer seed to use.</param>
    public void SetDeterministicSeed(int seed)
    {
        CurrentSeed = seed;
        _masterRandomGenerator = new System.Random(CurrentSeed);
        Debug.Log($"<color=cyan>[PatternSeedManager]</color> Deterministic seed set: <b>{CurrentSeed}</b>");
    }

    /// <summary>
    /// Generates a new, non-deterministic seed based on the system clock.
    /// </summary>
    public void SetNewRandomSeed()
    {
        CurrentSeed = (int)System.DateTime.Now.Ticks;
        _masterRandomGenerator = new System.Random(CurrentSeed);
        Debug.Log($"<color=cyan>[PatternSeedManager]</color> New random seed generated: <b>{CurrentSeed}</b>");
    }

    /// <summary>
    /// Returns the next random integer within a specified range from the master generator.
    /// </summary>
    public int GetNext(int min, int max)
    {
        // The generator is initialized in Awake, so a null check is a fallback.
        if (_masterRandomGenerator == null) 
        {
             Debug.LogWarning("[PatternSeedManager] Generator not initialized! Setting new random seed.");
             SetNewRandomSeed();
        }
        return _masterRandomGenerator.Next(min, max);
    }

    /// <summary>
    /// Returns the next random float between 0.0 (inclusive) and 1.0 (exclusive) from the master generator.
    /// </summary>
    public float GetNextFloat()
    {
        if (_masterRandomGenerator == null)
        {
            Debug.LogWarning("[PatternSeedManager] Generator not initialized! Setting new random seed.");
            SetNewRandomSeed();
        }
        return (float)_masterRandomGenerator.NextDouble();
    }
}
