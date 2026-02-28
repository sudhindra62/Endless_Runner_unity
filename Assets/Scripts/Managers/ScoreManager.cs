
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.Register(this);
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<ScoreManager>();
    }

    public int CurrentScore { get; private set; }

    public void ResetScore()
    {
        CurrentScore = 0;
    }

    public void Pause()
    {
        // In a real implementation, this would pause score accumulation.
    }

    public void Resume()
    {
        // In a real implementation, this would resume score accumulation.
    }
}
