using UnityEngine;
using System.Collections.Generic;

public class MultiplayerGhostManager : MonoBehaviour
{
    public static MultiplayerGhostManager Instance { get; private set; }

    [Header("Dependencies")]
    [SerializeField] private GhostRunPlayback ghostPlayback;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private IntegrityManager integrityManager;
    [SerializeField] private GameObject raceUI;

    private GhostRunData currentGhostRun;
    private bool isRaceActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartGhostRace(GhostRunData ghostData)
    {
        if (ghostData == null || !ghostData.IsValid(integrityManager.GetTheoreticalMaxScore()))
        {
            Debug.LogWarning("Invalid ghost data. Race cannot start.");
            return;
        }

        currentGhostRun = ghostData;
        ghostPlayback.LoadGhostRun(currentGhostRun.dataPoints);
        isRaceActive = true;

        if (raceUI != null) raceUI.SetActive(true);
    }

    private void Update()
    {
        if (!isRaceActive) return;

        // Track player vs. ghost progress and update UI
        // This is a simplified example. A more robust implementation would compare distance or score.
        if (playerMovement.transform.position.z > ghostPlayback.transform.position.z)
        {
            // Display "You are ahead!" message
        }
    }

    public void EndGhostRace()
    {
        isRaceActive = false;
        ghostPlayback.StopPlayback();
        if (raceUI != null) raceUI.SetActive(false);
    }

    public void ToggleGhostVisibility(bool isVisible)
    {
        ghostPlayback.gameObject.SetActive(isVisible);
    }

    // Method to simulate long sessions and test for memory leaks
    public void SimulateLongSession(int raceCount, GhostRunData testData)
    {
        for (int i = 0; i < raceCount; i++)
        {
            StartGhostRace(testData);
            // In a real test, we would wait for the race to finish
            EndGhostRace();
        }
    }
}
