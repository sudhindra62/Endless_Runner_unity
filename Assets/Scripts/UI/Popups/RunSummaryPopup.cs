
using UnityEngine;

public class RunSummaryPopup : MonoBehaviour
{
    public void ShowSummary(RunSessionData runData)
    {
        // Logic to populate the run summary UI with data
        gameObject.SetActive(true);
    }

    public void HideSummary()
    {
        gameObject.SetActive(false);
    }
}
