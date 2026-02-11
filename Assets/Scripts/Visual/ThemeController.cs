using UnityEngine;

public class ThemeController : MonoBehaviour
{
    public Camera cam;
    public Color slowColor = Color.gray;
    public Color fastColor = Color.cyan;
    public float transitionSpeed = 2f;

    void Start()
    {
        if (!cam) cam = Camera.main;
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * 0.05f, 1f);
        cam.backgroundColor = Color.Lerp(cam.backgroundColor,
            Color.Lerp(slowColor, fastColor, t),
            Time.deltaTime * transitionSpeed);
    }
}
