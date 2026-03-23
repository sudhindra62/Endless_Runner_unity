
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Manages the user interface for viewing and interacting with replays.
/// Displays lists of available replays and provides playback control buttons.
/// </summary>
public class ReplayViewerUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject replayListPanel;
    [SerializeField] private GameObject playbackControlsPanel;

    [Header("UI Elements")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button speed1xButton;
    [SerializeField] private Button speed2xButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform replayButtonContainer;
    [SerializeField] private GameObject replayButtonPrefab;

    private ReplayManager replayManager;
    private ReplayPlaybackController playbackController;

    private void Start()
    {
        replayManager = ReplayManager.Instance;
        // The playback controller might be on a different object, so we find it.
        playbackController = FindFirstObjectByType<ReplayPlaybackController>();

        // Hook up button listeners
        playButton.onClick.AddListener(OnPlay); 
        pauseButton.onClick.AddListener(OnPause);
        speed1xButton.onClick.AddListener(() => OnSetSpeed(1f));
        speed2xButton.onClick.AddListener(() => OnSetSpeed(2f));
        closeButton.onClick.AddListener(ClosePlayback);
        
        // Initially, show the list and hide the controls
        replayListPanel.SetActive(true);
        playbackControlsPanel.SetActive(false);
        
        PopulateReplayList();
    }

    public void PopulateReplayList()
    {
        // Clear existing buttons
        foreach (Transform child in replayButtonContainer)
        {
            Destroy(child.gameObject);
        }

        List<ReplayData> replays = replayManager.GetAllRecentReplays();
        foreach (ReplayData replay in replays)
        {
            GameObject buttonGO = Instantiate(replayButtonPrefab, replayButtonContainer);
            // Assuming the prefab has a text component to show replay info
            Text buttonText = buttonGO.GetComponentInChildren<Text>();
            buttonText.text = $"Score: {replay.score} - {replay.dateRecorded.ToShortDateString()}";
            
            Button button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() => OnSelectReplay(replay.replayId));
        }
    }

    private void OnSelectReplay(string replayId)
    {
        replayManager.PlayReplay(replayId);
        replayListPanel.SetActive(false);
        playbackControlsPanel.SetActive(true);
    }

    private void OnPlay()
    {
        if(playbackController != null) playbackController.Play();
    }

    private void OnPause()
    {
        if(playbackController != null) playbackController.Pause();
    }

    private void OnSetSpeed(float speed)
    {
        if(playbackController != null) playbackController.SetSpeed(speed);
    }

    private void ClosePlayback()
    {
        // Logic to stop the replay and return to the menu
        // This might involve destroying the ghost object and loading the main menu scene.
        replayListPanel.SetActive(true);
        playbackControlsPanel.SetActive(false);
    }
}
