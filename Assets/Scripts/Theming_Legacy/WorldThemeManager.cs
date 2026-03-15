using UnityEngine;

public class WorldThemeManager : MonoBehaviour
{
    public static WorldThemeManager Instance { get; private set; }

    // This would be a list of all decorative objects in the scene that can be enabled/disabled.
    [SerializeField]
    private GameObject[] environmentDecorations;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDecorationDensity(float density)
    {
        if (environmentDecorations == null) return;

        density = Mathf.Clamp01(density);
        int decorationsToActivate = (int)(environmentDecorations.Length * density);

        for (int i = 0; i < environmentDecorations.Length; i++)
        {
            if(environmentDecorations[i] != null)
            {
                environmentDecorations[i].SetActive(i < decorationsToActivate);
            }
        }

        Debug.Log($"Environment decoration density set to {density * 100}% ({decorationsToActivate}/{environmentDecorations.Length} objects active).");
    }
}
