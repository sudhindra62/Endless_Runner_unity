using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class HomeManager : MonoBehaviour
{
    [Header("Texts")]
    public TMP_Text coinsText;
    public TMP_Text bestScoreText;
    public TMP_Text challengeText;
    public TMP_Text rewardText;

    [Header("Progress")]
    public Image progressBar;

    private int dailyTarget = 100;
    private int rewardCoins = 200;

    void Start()
    {
        LoadStats();
        SetupChallenge();
    }

    void LoadStats()
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);

        coinsText.text = "Coins: " + coins;
        bestScoreText.text = "Best: " + bestScore;
    }

    void SetupChallenge()
    {
        challengeText.text = "Collect " + dailyTarget + " Coins";
        rewardText.text = "Reward: +" + rewardCoins + " Coins";

        progressBar.fillAmount = 0f;
    }

    public void UpdateChallengeProgress(int collected)
    {
        progressBar.fillAmount = Mathf.Clamp01((float)collected / dailyTarget);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }
}
