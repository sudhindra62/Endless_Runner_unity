
using UnityEngine;
using UnityEngine.UI;

public class FriendsPanelUI : MonoBehaviour
{
    [Header("Tabs")]
    [SerializeField] private GameObject friendsListTab;
    [SerializeField] private GameObject addFriendTab;
    [SerializeField] private GameObject challengesTab;

    [Header("UI Elements")]
    [SerializeField] private InputField friendCodeInput;
    [SerializeField] private Button addFriendButton;

    private void Start()
    {
        addFriendButton.onClick.AddListener(OnAddFriendClicked);
        ShowFriendsList();
    }

    public void ShowFriendsList()
    {
        friendsListTab.SetActive(true);
        addFriendTab.SetActive(false);
        challengesTab.SetActive(false);
    }

    public void ShowAddFriend()
    {
        friendsListTab.SetActive(false);
        addFriendTab.SetActive(true);
        challengesTab.SetActive(false);
    }

    public void ShowChallenges()
    {
        friendsListTab.SetActive(false);
        addFriendTab.SetActive(false);
        challengesTab.SetActive(true);
    }

    private void OnAddFriendClicked()
    {
        string friendCode = friendCodeInput.text;
        if (!string.IsNullOrEmpty(friendCode))
        {
            FriendManager.Instance.AddFriend(friendCode);
            friendCodeInput.text = "";
            ShowFriendsList(); // Switch back to the list after adding
        }
    }
}
