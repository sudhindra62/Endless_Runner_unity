
using UnityEngine;
using UnityEngine.Playables;

namespace EndlessRunner.Cinematics
{
    public class PreRunCinematicManager : MonoBehaviour
    {        public static PreRunCinematicManager Instance;

        public PlayableDirector cinematicDirector;
        public GameObject tapToRunText;

        private void Awake()
        {
            if (Instance == null)
            {                Instance = this;
            }
            else
            {                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Show "Tap to Run" on the home screen
            tapToRunText.SetActive(true);
            cinematicDirector.gameObject.SetActive(false);
        }

        public void PlayCinematic()
        {            tapToRunText.SetActive(false);
            cinematicDirector.gameObject.SetActive(true);
            cinematicDirector.Play();
            cinematicDirector.stopped += OnCinematicFinished;
        }

        private void OnCinematicFinished(PlayableDirector director)
        {
            director.stopped -= OnCinematicFinished;
            // This is where you would transition to the game starting
            Debug.Log("Cinematic finished, starting the game...");
            // For example, you might call a method on a GameManager
            // GameManager.Instance.StartGame();
        }
    }
}
