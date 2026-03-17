using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TutorialStepUI : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Button continueButton;

    public void ShowStep(string title, string description, Action onContinue)
    {
        titleText.text = title;
        descriptionText.text = description;
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => onContinue?.Invoke());
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
