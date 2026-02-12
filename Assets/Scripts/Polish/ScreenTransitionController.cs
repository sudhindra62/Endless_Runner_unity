
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages a smooth fade-to-black transition between scenes.
/// Uses a full-screen UI Image with a CanvasGroup to control the fade effect.
/// 
/// --- Inspector Setup ---
/// 1. Create a new Canvas in your starting scene. Set it to 'Screen Space - Overlay' and give it a high sort order.
/// 2. Add a child UI Image to the Canvas. Set its color to black and stretch it to fill the entire screen.
/// 3. Attach this script to the parent Canvas GameObject.
/// 4. Drag the CanvasGroup (or let the script add one) and the black Image to the inspector fields.
/// 5. Make this a persistent singleton by calling DontDestroyOnLoad(gameObject) if needed.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class ScreenTransitionController : MonoBehaviour
{
    public static ScreenTransitionController Instance { get; private set; }

    [Header("Configuration")]
    [SerializeField] private float fadeDuration = 0.5f;
    
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Fades out the current scene, loads a new one, and then fades back in.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {
        // Fade Out
        yield return StartCoroutine(Fade(1f));

        // Load Scene
        SceneManager.LoadScene(sceneName);

        // Fade In
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = canvasGroup.alpha;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha;
    }
}
