
using UnityEngine;
using TMPro;

    public class LeaderboardItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI rankText;
        [SerializeField] private TextMeshProUGUI playerNameText;
        [SerializeField] private TextMeshProUGUI scoreText;

        public void Setup(int rank, LeaderboardEntry entry)
        {
            rankText.text = rank.ToString();
            playerNameText.text = entry.playerName;
            scoreText.text = entry.score.ToString();
        }
    }
