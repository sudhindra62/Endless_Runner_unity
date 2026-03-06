
using UnityEngine;
using UnityEngine.UI;

public class ChallengeNotificationUI : MonoBehaviour
{
    [SerializeField] private Text notificationText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;

    private Challenge currentChallenge;

    private void Start()
    {
        acceptButton.onClick.AddListener(AcceptChallenge);
        declineButton.onClick.AddListener(DeclineChallenge);
        gameObject.SetActive(false);
    }

    public void DisplayChallenge(Challenge challenge)
    {
        currentChallenge = challenge;
        notificationText.text = $"{challenge.challengerID} challenged you to beat their {challenge.type} of {challenge.valueToBeat}!";
        gameObject.SetActive(true);
    }

    private void AcceptChallenge()
    {
        // Logic to start the challenge run
        Debug.Log($"Accepted challenge {currentChallenge.challengeID}");
        gameObject.SetActive(false);
    }

    private void DeclineChallenge()
    {
        Debug.Log($"Declined challenge {currentChallenge.challengeID}");
        gameObject.SetActive(false);
    }
}
