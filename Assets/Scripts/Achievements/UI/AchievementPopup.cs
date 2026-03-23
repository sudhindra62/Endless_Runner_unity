
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


    /// <summary>
    /// Displays a notification popup when an achievement is unlocked.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class AchievementPopup : MonoBehaviour
    {
        [Header("UI Element References")]
        [SerializeField] private Text achievementNameText;
        [SerializeField] private Text achievementDescriptionText;
        [SerializeField] private Image achievementIcon;

        [Header("Animation & Display Settings")]
        [SerializeField] private float fadeInDuration = 0.5f;
        [SerializeField] private float displayDuration = 3f;
        [SerializeField] private float fadeOutDuration = 0.5f;

        private CanvasGroup _canvasGroup;
        private Coroutine _displayCoroutine;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Start()
        {
            _canvasGroup.alpha = 0;
            gameObject.SetActive(false);
            AchievementManager.OnAchievementUnlocked += ShowPopup;
        }

        private void OnDestroy()
        {
            AchievementManager.OnAchievementUnlocked -= ShowPopup;
        }

        public void ShowPopup(Achievement achievement)
        {
            if (achievement == null) return;

            achievementNameText.text = achievement.Name;
            achievementDescriptionText.text = achievement.Description;
            achievementIcon.sprite = achievement.Badge;

            if (_displayCoroutine != null)
            {
                StopCoroutine(_displayCoroutine);
            }
            _displayCoroutine = StartCoroutine(DisplayAndHideSequence());
        }

        private IEnumerator DisplayAndHideSequence()
        {
            gameObject.SetActive(true);
            yield return AnimateCanvasGroup(0f, 1f, fadeInDuration);
            yield return new WaitForSeconds(displayDuration);
            yield return AnimateCanvasGroup(1f, 0f, fadeOutDuration);
            gameObject.SetActive(false);
        }

        private IEnumerator AnimateCanvasGroup(float startAlpha, float endAlpha, float duration)
        {
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                _canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
                yield return null;
            }
            _canvasGroup.alpha = endAlpha;
        }
    }

