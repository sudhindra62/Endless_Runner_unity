using UnityEngine;

/// <summary>
/// Triggers that can advance a tutorial step.
/// </summary>
public enum TutorialTrigger
{
    None, 
    SwipeLeft,
    SwipeRight,
    SwipeUp,
    ButtonPress
}

/// <summary>
/// Data structure for a single step in the tutorial sequence.
/// Global scope.
/// </summary>
[System.Serializable]
public class TutorialStep
{
    public string title;
    [TextArea(3, 10)]
    public string instructionText;
    public TutorialTrigger trigger;
    public Sprite icon;
}
