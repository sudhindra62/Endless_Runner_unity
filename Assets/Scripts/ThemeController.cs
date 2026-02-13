using UnityEngine;

public class ThemeController : MonoBehaviour
{
    public Camera cam;
    public Color slowColor = Color.gray;
    public Color fastColor = Color.cyan;
    public float transitionSpeed = 2f;

    private Color currentColor;

    void Start()
    {
        if (!cam) cam = Camera.main;
        currentColor = cam.backgroundColor;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * 0.05f, 1f);
        Color targetColor = Color.Lerp(slowColor, fastColor, t);
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * transitionSpeed);
        cam.backgroundColor = currentColor;
    }
}
