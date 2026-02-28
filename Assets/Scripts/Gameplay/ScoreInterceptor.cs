using UnityEngine;

public class ScoreInterceptor : MonoBehaviour
{
    private ScoreManager _scoreManager;

    private void Start()
    {
        _scoreManager = ServiceLocator.Get<ScoreManager>();
    }

    public void InterceptScore(int baseScore)
    {
        // For now, it just passes the score through.
        // Later, we can add logic here to modify the score.
        if (_scoreManager != null) {
            _scoreManager.AddScore(baseScore);
        }
    }
}
