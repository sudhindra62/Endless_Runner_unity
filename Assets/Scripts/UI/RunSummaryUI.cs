
using TMPro;
using UnityEngine;

public class RunSummaryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI bestTimeText;

    private PlayerDataManager playerDataManager;

    private void Start()
    {
        playerDataManager = ServiceLocator.Get<PlayerDataManager>();
        Hide();
    }

    public void Show(RunSessionData runSessionData)
    {
        gameObject.SetActive(true);

        scoreText.text = "Score: " + runSessionData.score;
        timeText.text = "Time: " + runSessionData.time.ToString("F2");

        bestScoreText.text = "Best Score: " + playerDataManager.GetBestScore();
        bestTimeText.text = "Best Time: " + playerDataManager.GetBestTime().ToString("F2");
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
