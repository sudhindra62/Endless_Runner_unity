
using UnityEngine;

    public class TutorialUI : MonoBehaviour
    {
        public void ShowPanel() { gameObject.SetActive(true); }
        public void HidePanel() { gameObject.SetActive(false); }
        public void ShowStep(int stepIndex, System.Action onContinue = null) { Debug.Log($"Tutorial Step: {stepIndex}"); }
        public void Hide() => HidePanel();
    }
