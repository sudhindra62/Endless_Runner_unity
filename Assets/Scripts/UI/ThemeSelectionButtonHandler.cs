using UnityEngine;
using UnityEngine.SceneManagement;

public class ThemeSelectionButtonHandler : MonoBehaviour
{
    public void OnSelectThemePressed()
    {
        SceneManager.LoadScene("ThemeSelection");
    }
}
