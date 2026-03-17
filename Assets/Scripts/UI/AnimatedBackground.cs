
using UnityEngine;
using UnityEngine.UI;

namespace EndlessRunner.UI
{
    public class AnimatedBackground : MonoBehaviour
    {
        public Sprite[] frames;
        public float frameRate = 12f;

        private Image image;
        private int currentFrame;
        private float timer;

        private void Awake()
        {
            image = GetComponent<Image>();
        }

        private void Update()
        {
            if (frames.Length == 0) return;

            timer += Time.deltaTime;
            if (timer >= 1f / frameRate)
            {
                timer = 0;
                currentFrame = (currentFrame + 1) % frames.Length;
                image.sprite = frames[currentFrame];
            }
        }
    }
}
