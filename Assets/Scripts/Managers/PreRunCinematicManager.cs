using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

/// <summary>
/// Manages the pre-run cinematic sequence and transition to gameplay.
/// Global scope.
/// </summary>
public class PreRunCinematicManager : Singleton<PreRunCinematicManager>
{
    [Header("UI & Visuals")]
    public PlayableDirector cinematicDirector;
    public GameObject tapToRunObject;

    [Header("Events")]
    public UnityEvent OnCinematicFinished;

    protected override void Awake()
    {
        base.Awake();
        if (tapToRunObject) tapToRunObject.SetActive(true);
    }

    public void PlayCinematic()
    {
        if (tapToRunObject) tapToRunObject.SetActive(false);

        if (cinematicDirector != null)
        {
            cinematicDirector.gameObject.SetActive(true);
            cinematicDirector.Play();
            cinematicDirector.stopped += OnDirectorStopped;
        }
        else
        {
            OnCinematicFinished.Invoke();
        }
    }

    private void OnDirectorStopped(PlayableDirector director)
    {
        director.stopped -= OnDirectorStopped;
        OnCinematicFinished.Invoke();
        if (GameManager.Instance != null) GameManager.Instance.StartGame();
    }
}
