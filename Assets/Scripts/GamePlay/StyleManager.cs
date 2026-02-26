using UnityEngine;

public class StyleManager : MonoBehaviour
{
    public static StyleManager Instance { get; private set; }

    [Header("Style Settings")]
    [Tooltip("The amount of style points gained for a perfect dodge.")]
    public float perfectDodgeBonus = 10f;
    [Tooltip("The rate at which the style meter decays over time.")]
    public float styleDecayRate = 2f;
    [Tooltip("The maximum amount of style points.")]
    public float maxStyle = 100f;

    [Header("Multiplier Settings")]
    [Tooltip("The style threshold required to activate the first multiplier level.")]
    public float multiplierThreshold1 = 50f;
    [Tooltip("The style threshold required to activate the second multiplier level.")]
    public float multiplierThreshold2 = 90f;

    public float CurrentStyle { get; private set; }
    public int ScoreMultiplier { get; private set; } = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Decay the style meter over time
        if (CurrentStyle > 0)
        {
            CurrentStyle -= styleDecayRate * Time.deltaTime;
            CurrentStyle = Mathf.Max(0, CurrentStyle);
        }

        UpdateScoreMultiplier();
    }

    public void PerfectDodge()
    {
        CurrentStyle += perfectDodgeBonus;
        CurrentStyle = Mathf.Min(CurrentStyle, maxStyle);
    }

    private void UpdateScoreMultiplier()
    {
        if (CurrentStyle >= multiplierThreshold2)
        {
            ScoreMultiplier = 3;
        }
        else if (CurrentStyle >= multiplierThreshold1)
        {
            ScoreMultiplier = 2;
        }
        else
        {
            ScoreMultiplier = 1;
        }
    }
}
