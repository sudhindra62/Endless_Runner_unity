
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

namespace EndlessRunner.Managers
{
    public class PreRunCinematicManager : MonoBehaviour
    {
        public static PreRunCinematicManager Instance;

        public PlayableDirector cinematicDirector;
        public GameObject tapToRunObject;
        public UnityEvent OnCinematicFinished;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            // Show the "Tap to Run" object on start
            if(tapToRunObject) tapToRunObject.SetActive(true);
        }

        public void PlayCinematic()
        {
            if (cinematicDirector != null)
            {
                cinematicDirector.Play();
                cinematicDirector.stopped += OnDirectorStopped;
            }
            else
            {
                // If no cinematic, just start the game
                OnCinematicFinished.Invoke();
            }

            // Hide the "Tap to Run" object
            if(tapToRunObject) tapToRunObject.SetActive(false);
        }

        private void OnDirectorStopped(PlayableDirector director)
        {
            OnCinematicFinished.Invoke();
            director.stopped -= OnDirectorStopped;
        }
    }
}
