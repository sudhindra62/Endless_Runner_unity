
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a single frame of recorded replay data.
/// </summary>
[Serializable]
public struct ReplayFrame
{
    public float timestamp;
    public Vector3 position;
    public Quaternion rotation; // Added for more accurate playback
    public PlayerAction action; // Jump, Slide, etc.
}

/// <summary>
/// Represents a complete replay of a single run, containing all recorded frames.
/// </summary>
[Serializable]
public class ReplayData
{
    public string replayId;
    public DateTime dateRecorded;
    public int score;
    public float distance;
    public List<ReplayFrame> frames;

    public ReplayData()
    {
        frames = new List<ReplayFrame>();
        replayId = Guid.NewGuid().ToString();
        dateRecorded = DateTime.UtcNow;
    }
}

/// <summary>
/// Enum representing discrete player actions for recording.
/// </summary>
public enum PlayerAction
{
    None,
    Jump,
    Slide,
    LaneChangeLeft,
    LaneChangeRight,
    PowerUpActivate
}
