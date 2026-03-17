using UnityEngine;

namespace EndlessRunner.Environment
{
    public class Lighting : MonoBehaviour
    {
        private Light directionalLight;

        private void Start()
        {
            directionalLight = GetComponent<Light>();
        }

        public void ApplyTheme(ThemeConfig theme)
        {
            // Set skybox
            if (theme.skyboxMaterial != null)
            {
                RenderSettings.skybox = theme.skyboxMaterial;
            }

            // Set fog
            RenderSettings.fog = true;
            RenderSettings.fogColor = theme.fogColor;
            RenderSettings.fogDensity = theme.fogDensity;

            // You can also adjust the directional light's properties here, for example:
            // directionalLight.color = theme.lightColor;
            // directionalLight.intensity = theme.lightIntensity;
        }
    }
}
