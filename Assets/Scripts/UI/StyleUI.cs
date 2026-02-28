using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StyleUI : MonoBehaviour
{
    [Header("UI References")]
    public Image styleMeterImage;
    public TextMeshProUGUI multiplierText;

    void Update()
    {
        if (StyleManager.Instance != null)
        {
            // Update style meter
            if (styleMeterImage != null)
            {
                styleMeterImage.fillAmount = StyleManager.Instance.currentStyle / StyleManager.Instance.maxStyle;
            }

            // Update multiplier text
            if (multiplierText != null)
            {
                multiplierText.text = "x" + StyleManager.Instance.CurrentMultiplier.ToString();
            }
        }
    }
}
