using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

/// <summary>
/// A singleton controller for managing scene loading and transitions.
/// Global scope for maximum project-wide accessibility.
/// </summary>
public class SceneLoader : Singleton<SceneLoader>
{
    [Header("Loading Screen Configuration")]
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    public event Action<float> OnLoadProgressChanged;

    protected override void Awake()
    {
        base.Awake();
        if (loadingScreenCanvasGroup != null)
        {
            loadingScreenCanvasGroup.alpha = 0;
            loadingScreenCanvasGroup.interactable = false;
            loadingScreenCanvasGroup.blocksRaycasts = false;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    public void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        yield return StartCoroutine(Fade(1f));
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            OnLoadProgressChanged?.Invoke(progress);
            yield return null;
        }
        yield return StartCoroutine(Fade(0f));
    }

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
