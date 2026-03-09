/// <summary>
/// Defines the different operational states for the CameraManager.
/// </summary>
public enum CameraState
{
    /// <summary>The camera is not active or is pending initialization.</summary>
    None,
    /// <summary>The camera is smoothly following a designated target.</summary>
    Following,
    /// <summary>The camera is locked at a specific position and rotation.</summary>
    Fixed,
    /// <summary>The camera is being controlled by a cinematic sequence (e.g., Timeline).</summary>
    Cinematic
}
