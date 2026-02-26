using UnityEngine;

public class MilestoneManager : MonoBehaviour
{
    public static MilestoneManager Instance { get; private set; }

    // Milestone Properties
    public float TotalDistanceTraveled { get; private set; }
    public int TotalCoinsCollected { get; private set; }
    public int TotalShieldsUsed { get; private set; }
    public int TotalJumps { get; private set; }
    public int TotalSlides { get; private set; }

    // PlayerPrefs Keys
    private const string DistanceKey = "TotalDistanceTraveled";
    private const string CoinsKey = "TotalCoinsCollected";
    private const string ShieldsKey = "TotalShieldsUsed";
    private const string JumpsKey = "TotalJumps";
    private const string SlidesKey = "TotalSlides";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadMilestones();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Public methods to report milestones
    public void AddDistance(float distance)
    {
        TotalDistanceTraveled += distance;
        SaveMilestones();
    }

    public void AddCoins(int amount)
    {
        TotalCoinsCollected += amount;
        SaveMilestones();
    }

    public void ReportShieldUse()
    {
        TotalShieldsUsed++;
        SaveMilestones();
    }

    public void ReportJump()
    {
        TotalJumps++;
        SaveMilestones();
    }

    public void ReportSlide()
    {
        TotalSlides++;
        SaveMilestones();
    }

    // Save and Load Methods
    private void SaveMilestones()
    {
        PlayerPrefs.SetFloat(DistanceKey, TotalDistanceTraveled);
        PlayerPrefs.SetInt(CoinsKey, TotalCoinsCollected);
        PlayerPrefs.SetInt(ShieldsKey, TotalShieldsUsed);
        PlayerPrefs.SetInt(JumpsKey, TotalJumps);
        PlayerPrefs.SetInt(SlidesKey, TotalSlides);
        PlayerPrefs.Save();
    }

    private void LoadMilestones()
    {
        TotalDistanceTraveled = PlayerPrefs.GetFloat(DistanceKey, 0);
        TotalCoinsCollected = PlayerPrefs.GetInt(CoinsKey, 0);
        TotalShieldsUsed = PlayerPrefs.GetInt(ShieldsKey, 0);
        TotalJumps = PlayerPrefs.GetInt(JumpsKey, 0);
        TotalSlides = PlayerPrefs.GetInt(SlidesKey, 0);
    }

    // Method to reset all milestones (for debugging/testing)
    public void ResetMilestones()
    {
        TotalDistanceTraveled = 0;
        TotalCoinsCollected = 0;
        TotalShieldsUsed = 0;
        TotalJumps = 0;
        TotalSlides = 0;
        SaveMilestones();
    }
}
