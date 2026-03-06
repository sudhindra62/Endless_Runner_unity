
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EventCharactersPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject eventCharacterPanel;
    [SerializeField] private Button openPanelButton; // e.g., the home screen banner
    [SerializeField] private Button closePanelButton;
    [SerializeField] private Transform characterListContainer;
    [SerializeField] private GameObject characterUIPrefab; // A prefab to display a single event character

    void Start()
    {
        openPanelButton.onClick.AddListener(OpenPanel);
        closePanelButton.onClick.AddListener(ClosePanel);
        eventCharacterPanel.SetActive(false);
    }

    private void OpenPanel()
    {
        eventCharacterPanel.SetActive(true);
        PopulateCharacterList();
    }

    private void ClosePanel()
    {
        eventCharacterPanel.SetActive(false);
    }

    private void PopulateCharacterList()
    {
        // Clear existing list
        foreach (Transform child in characterListContainer)
        {
            Destroy(child.gameObject);
        }

        List<EventCharacterData> eventCharacters = EventCharacterManager.Instance.GetActiveEventCharacters();
        foreach (var character in eventCharacters)
        {
            GameObject characterUI = Instantiate(characterUIPrefab, characterListContainer);
            // Set up the character UI with preview, description, price, etc.
            // Add listeners for purchase/unlock buttons.
        }
    }
}
