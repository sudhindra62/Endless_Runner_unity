
using UnityEngine;


    /// <summary>
    /// Intercepts and potentially modifies the score before it is added to the ScoreManager.
    /// </summary>
    public class ScoreInterceptor : MonoBehaviour
    {
        private ScoreManager _scoreManager;

        private void Start()
        {
            _scoreManager = ServiceLocator.Get<ScoreManager>();
        }

        /// <summary>
        /// Intercepts the score, applies any modifications, and passes it to the ScoreManager.
        /// </summary>
        public void InterceptScore(int baseScore)
        {
            // Later, we can add logic here to modify the score.
            int modifiedScore = baseScore;

            if (_scoreManager != null)
            {
                _scoreManager.AddScore(modifiedScore);
            }
        }
    }

