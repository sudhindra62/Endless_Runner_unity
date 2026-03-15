
using UnityEngine;

namespace EndlessRunner.Data
{
    public enum TutorialTrigger
    {
        None,
        SwipeLeft,
        SwipeRight,
        SwipeUp,
        ButtonPressed
    }

    [System.Serializable]
    public class TutorialStep
    {
        public string instructionText;
        public Sprite instructionSprite;
        public TutorialTrigger trigger;
        public bool waitForButtonPress => trigger == TutorialTrigger.ButtonPressed;
    }
}
