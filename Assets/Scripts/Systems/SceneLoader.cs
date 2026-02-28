
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

/// <summary>
/// A singleton controller for managing scene loading and transitions.
/// It uses asynchronous loading to prevent the game from freezing and provides a smooth fade-in/fade-out transition.
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// The static singleton instance of the SceneController.
    /// </summary>
    public static SceneController Instance { get; private set; }

    [Header("Loading Screen Configuration")]
    [Tooltip("The CanvasGroup containing the loading screen UI. It will be faded in and out.")]
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;
    [Tooltip("The duration of the fade-in and fade-out animations in seconds.")]
    [SerializeField] private float fadeDuration = 0.5f;

    /// <summary>
    /// Fired during an asynchronous load operation to report the loading progress.
    /// The float value ranges from 0.0 (starting) to 1.0 (complete).
    /// </summary>
    public event Action<float> OnLoadProgressChanged;

    private void Awake()
    {
        // Standard singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Ensure the loading screen is initially hidden.
        if (loadingScreenCanvasGroup != null)
        {
            loadingScreenCanvasGroup.alpha = 0;
            loadingScreenCanvasGroup.interactable = false;
            loadingScreenCanvasGroup.blocksRaycasts = false;
        }
    }

    /// <summary>
    /// Loads a scene asynchronously with a fade transition.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    /// <summary>
    /// The coroutine that handles the entire scene loading process: fade out, load, and fade in.
    /// </summary>
    /// <param name="sceneName">The name of the scene to load.</param>
    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        yield return StartCoroutine(Fade(1f)); // Fade to black

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Update loading progress until the scene is almost ready
        while (!asyncLoad.isDone)
        {
            // The asyncLoad.progress value goes from 0 to 0.9. We remap it to 0-1 for the event.
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            OnLoadProgressChanged?.Invoke(progress);
            yield return null;
        }

        yield return StartCoroutine(Fade(0f)); // Fade back in
    }

    /// <summary>
    /// A coroutine to fade the loading screen's CanvasGroup in or out.
    /// </summary>
    /// <param name="targetAlpha">The target alpha value (0 for transparent, 1 for opaque).</param>
    private IEnumerator Fade(float targetAlpha)
    {
        if (loadingScreenCanvasGroup == null) yield break;

        loadingScreenCanvasGroup.interactable = true;
        loadingScreenCanvasGroup.blocksRaycasts = true;

        float startAlpha = loadingScreenCanvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            loadingScreenCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        loadingScreenCanvasGroup.alpha = targetAlpha;
        
        if (targetAlpha == 0)
        {
            loadingScreenCanvasGroup.interactable = false;
            loadingScreenCanvasGroup.blocksRaycasts = false;
        }
    }
}
