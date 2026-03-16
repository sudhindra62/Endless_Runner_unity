using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void Start()
    {
        ThemeSO currentTheme = ThemeManager.Instance.GetCurrentTheme();
        if (currentTheme != null)
        {
            // You can use theme-specific obstacle visuals here if needed
            // For example, by enabling/disabling child objects or changing materials
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // End game or apply penalty to the player
        }
    }
}
