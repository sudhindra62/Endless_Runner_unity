
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : UIPanel
{
    public override UIPanelType PanelType => UIPanelType.GameOver;

    [SerializeField] private Text _finalScoreText;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _mainMenuButton;

    private void Start()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnEnable()
    {
        _finalScoreText.text = "Final Score: " + ScoreManager.Instance.GetScore();
    }

    private void OnRestartButtonClicked()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.Playing);
        GameUIManager.Instance.HidePanel(UIPanelType.GameOver);
        GameUIManager.Instance.ShowPanel(UIPanelType.InGame);
    }

    private void OnMainMenuButtonClicked()
    {
        GameManager.Instance.ChangeState(GameManager.GameState.MainMenu);
        GameUIManager.Instance.HidePanel(UIPanelType.GameOver);
        GameUIManager.Instance.ShowPanel(UIPanelType.MainMenu);
    }
}
