using UnityEngine;
using UnityEngine.UI;

public class MilestoneUI : MonoBehaviour
{
    [Header("UI Text References")]
    public Text distanceText;
    public Text coinsText;
    public Text shieldsText;
    public Text jumpsText;
    public Text slidesText;

    private void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (MilestoneManager.Instance == null) return;

        if (distanceText != null)
        {
            distanceText.text = "Distance: " + MilestoneManager.Instance.TotalDistanceTraveled.ToString("F2");
        }
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + MilestoneManager.Instance.TotalCoinsCollected.ToString();
        }
        if (shieldsText != null)
        {
            shieldsText.text = "Shields Used: " + MilestoneManager.Instance.TotalShieldsUsed.ToString();
        }
        if (jumpsText != null)
        {
            jumpsText.text = "Jumps: " + MilestoneManager.Instance.TotalJumps.ToString();
        }
        if (slidesText != null)
        {
            slidesText.text = "Slides: " + MilestoneManager.Instance.TotalSlides.ToString();
        }
    }
}
