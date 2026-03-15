
using UnityEngine;

namespace EndlessRunner.Data
{
    /// <summary>
    /// Defines a single step in a tutorial sequence.
    /// </summary>
    [System.Serializable]
    public class TutorialStep
    {
        [Tooltip("The instructional text to display to the player.")]
        public string instructionText;

        [Tooltip("The duration to display the instruction, if no action is required.")]
        public float duration = 3f;

        [Tooltip("The specific action the player must perform to complete this step.")]
        public TutorialAction requiredAction;
    }

    /// <summary>
    /// The specific action a player must perform to complete a tutorial step.
    /// </summary>
    public enum TutorialAction
    {
        None,
        SwipeLeft,
        SwipeRight,
        Jump,       // Swipe Up
        Slide       // Swipe Down
    }
}
