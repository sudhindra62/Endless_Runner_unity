
using UnityEngine;

namespace EndlessRunner.UI
{
    [System.Serializable]
    public class TutorialStep
    {
        public string title;
        [TextArea(3, 10)]
        public string description;
        public Sprite image;
    }
}
