using UnityEngine;

public class StyleBonusCalculator : MonoBehaviour
{
    private ScoreInterceptor _scoreInterceptor;

    private void Start()
    {
        _scoreInterceptor = ServiceLocator.Get<ScoreInterceptor>();
    }

    public void CalculateAndApplyBonus(int baseScore)
    {
        // For now, it just passes the score through to the interceptor.
        if (_scoreInterceptor != null) {
            _scoreInterceptor.InterceptScore(baseScore);
        }
    }
}
