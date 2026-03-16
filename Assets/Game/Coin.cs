using UnityEngine;

public class Coin : MonoBehaviour
{
    private void Start()
    {
        ThemeSO currentTheme = ThemeManager.Instance.GetCurrentTheme();
        if (currentTheme != null)
        {
            // You can use theme-specific coin visuals here if needed
            // For example, by enabling/disabling child objects or changing materials
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.Instance.AddScore(10);
            // Add particle effect and sound effect here
            Destroy(gameObject);
        }
    }
}
