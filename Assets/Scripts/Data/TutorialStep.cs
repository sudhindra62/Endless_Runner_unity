
using UnityEngine;

namespace EndlessRunner.Data
{
    public enum TutorialTrigger
    {
        None, // Used for steps that only require a button press to continue
        SwipeLeft,
        SwipeRight,
        SwipeUp
    }

    [System.Serializable]
    public class TutorialStep
    {
        [TextArea(3, 10)]
        public string instructionText;
        public TutorialTrigger trigger;
        // Potentially add references to UI elements to highlight, etc.
    }
}
