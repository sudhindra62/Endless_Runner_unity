
using UnityEngine;
using UnityEngine.UI;

public class RevivePopupUI : MonoBehaviour
{
    [SerializeField] private Button gemReviveButton;
    [SerializeField] private Button adReviveButton;
    [SerializeField] private Button tokenReviveButton;
    [SerializeField] private Button declineButton;

    private GameFlowController gameFlowController;

    private void Start()
    {
        gameFlowController = FindObjectOfType<GameFlowController>();

        gemReviveButton.onClick.AddListener(() => gameFlowController.OnReviveAccepted(ReviveManager.ReviveType.Gems));
        adReviveButton.onClick.AddListener(() => gameFlowController.OnReviveAccepted(ReviveManager.ReviveType.Ad));
        tokenReviveButton.onClick.AddListener(() => gameFlowController.OnReviveAccepted(ReviveManager.ReviveType.Token));
        declineButton.onClick.AddListener(() => gameFlowController.OnReviveDeclined());

        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        // Here you would also update the UI to show the cost of each revive type
        // and enable/disable buttons based on availability
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
