
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using System.Collections.Generic;

// This script is the final, high-accuracy implementation. It procedurally constructs the entire scene
// from code with a high degree of detail, matching the reference image.
public class MainSceneSetup : MonoBehaviour
{
    #region Configuration
    [Header("Asset & Prefab Configuration")]
    [Tooltip("Enable this to have the script create placeholder primitives for visual elements.")]
    public bool useProceduralPlaceholders = true;
    [SerializeField] private GameObject futuristicPlayerPrefab; // Optional: For using an actual model
    [SerializeField] private GameObject robotDogPrefab;         // Optional: For using an actual model

    [Header("Scene References (Optional)")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text comboText;
    [SerializeField] private Image comboProgressBar;
    #endregion

    #region Materials
    // Materials created at runtime to match the reference image
    private Material _skyMaterial, _pinkTreeMat, _blueTreeMat, _trainGraffitiMat, _playerTrailMat, 
                     _robotBodyMat, _robotGlowMat, _coinMat, _buildingMat, _windowMat, 
                     _railMat, _dataRiverMat, _uiPanelMat, _uiBarMat;
    #endregion

    #region Unity Lifecycle
    void Awake()
    {
        // Instantiate core managers if they don't exist.
        InstantiateManagers();
    }

    void Start()
    {
        // The main execution sequence.
        CreateRuntimeMaterials();
        ConfigureSceneSettings();
        if (useProceduralPlaceholders)
        {
            GenerateHighFidelityScene();
        }
    }
    #endregion

    #region Core Setup Methods
    private void InstantiateManagers()
    {
        // Add managers to the scene if they're missing, ensuring systems are ready.
        if (FindObjectOfType<LightingManager>() == null) new GameObject("LightingManager").AddComponent<LightingManager>();
        if (FindObjectOfType<VFXManager>() == null) new GameObject("VFXManager").AddComponent<VFXManager>();
        if (FindObjectOfType<SkyboxManager>() == null) new GameObject("SkyboxManager").AddComponent<SkyboxManager>();
        if (FindObjectOfType<PostProcessingManager>() == null) new GameObject("PostProcessingManager").AddComponent<PostProcessingManager>();
        if (FindObjectOfType<EnvironmentAnimationManager>() == null) new GameObject("EnvironmentAnimationManager").AddComponent<EnvironmentAnimationManager>();
        if (FindObjectOfType<PerformanceManager>() == null) new GameObject("PerformanceManager").AddComponent<PerformanceManager>();
    }

    private void CreateRuntimeMaterials()
    {
        // Generate all materials programmatically with colors sampled from the reference image for accuracy.
        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");

        _skyMaterial = new Material(Shader.Find("Skybox/Procedural"));
        _skyMaterial.SetColor("_SkyTint", new Color(0.1f, 0.05f, 0.2f));
        _skyMaterial.SetColor("_GroundColor", new Color(0.8f, 0.4f, 0.9f));

        _pinkTreeMat = new Material(urpLit) { color = new Color(1f, 0.2f, 0.8f) };
        _pinkTreeMat.SetColor("_EmissionColor", _pinkTreeMat.color * 2.0f); _pinkTreeMat.EnableKeyword("_EMISSION");

        _blueTreeMat = new Material(urpLit) { color = new Color(0.3f, 0.8f, 1f) };
        _blueTreeMat.SetColor("_EmissionColor", _blueTreeMat.color * 2.0f); _blueTreeMat.EnableKeyword("_EMISSION");

        _playerTrailMat = new Material(Shader.Find("Legacy Shaders/Particles/Additive")) { color = new Color(1f, 0.8f, 0.2f, 0.7f) };
        
        _robotBodyMat = new Material(urpLit) { color = new Color(0.1f, 0.1f, 0.15f) };
        _robotGlowMat = new Material(urpLit) { color = Color.red };
        _robotGlowMat.SetColor("_EmissionColor", Color.red * 4.0f); _robotGlowMat.EnableKeyword("_EMISSION");

        _coinMat = new Material(urpLit) { color = new Color(1f, 0.85f, 0f) };
        _coinMat.SetColor("_EmissionColor", _coinMat.color); _coinMat.EnableKeyword("_EMISSION");

        _buildingMat = new Material(urpLit) { color = new Color(0.05f, 0.05f, 0.1f) };
        _windowMat = new Material(urpLit) { color = new Color(0.8f, 0.6f, 1f) };
        _windowMat.SetColor("_EmissionColor", new Color(0.8f, 0.6f, 1f) * 1.8f); _windowMat.EnableKeyword("_EMISSION");

        _railMat = new Material(urpLit) { color = new Color(0.2f, 0.2f, 0.25f) };
        _dataRiverMat = new Material(urpLit) { color = new Color(0f, 0.8f, 1f) };
        _dataRiverMat.SetColor("_EmissionColor", new Color(0f, 0.8f, 1f) * 1.5f); _dataRiverMat.EnableKeyword("_EMISSION");

        _uiPanelMat = new Material(urpLit) { color = new Color(0.1f, 0.15f, 0.3f, 0.6f) };
        _uiBarMat = new Material(urpLit) { color = new Color(0.2f, 0.9f, 0.9f) };
    }

    private void ConfigureSceneSettings()
    {
        // Configure global scene properties like lighting, reflections, and post-processing.
        RenderSettings.skybox = _skyMaterial;
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.1f, 0.1f, 0.2f);
        RenderSettings.ambientEquatorColor = new Color(0.8f, 0.4f, 0.9f);
        RenderSettings.ambientGroundColor = new Color(0.3f, 0.3f, 0.5f);
        RenderSettings.reflectionIntensity = 1.5f;

        PostProcessingManager.Instance?.SetBloom(true, 1.5f, 0.7f);
        PostProcessingManager.Instance?.SetColorGrading(true, -0.1f, 25f, 15f);
        PostProcessingManager.Instance?.SetDepthOfField(true, 50f, 2.0f);
        PostProcessingManager.Instance?.SetMotionBlur(true, 0.5f);
    }

    private void GenerateHighFidelityScene()
    {
        // Main method to construct all procedural game objects.
        CreatePlayerAndCompanion();
        CreateEnvironment();
        CreateUI();
        CreateVFX();
    }
    #endregion

    #region Procedural Generation

    private void CreatePlayerAndCompanion()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player") ?? GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player"; player.tag = "Player";
        player.transform.position = new Vector3(-1.5f, 1, 0);
        player.GetComponent<Renderer>().material.color = new Color(0.1f, 0.1f, 0.1f);

        // Add glowing back emblem
        GameObject backEmblem = GameObject.CreatePrimitive(PrimitiveType.Cube);
        backEmblem.transform.SetParent(player.transform);
        backEmblem.transform.localPosition = new Vector3(0, 0.4f, -0.3f);
        backEmblem.transform.localScale = new Vector3(0.3f, 0.3f, 0.1f);
        backEmblem.GetComponent<Renderer>().material = _blueTreeMat; // Re-use a glowing material

        // Refine player trail
        TrailRenderer trail = player.GetComponent<TrailRenderer>() ?? player.AddComponent<TrailRenderer>();
        trail.material = _playerTrailMat;
        trail.startWidth = 0.8f; trail.endWidth = 0.05f; trail.time = 1.2f;
        trail.minVertexDistance = 0.2f;

        // Create more detailed Robot Dog
        CreateRoboPanther(player.transform);
    }

    private void CreateRoboPanther(Transform target)
    {
        GameObject panther = new GameObject("RoboPanther_Companion");
        panther.AddComponent<FollowPlayer>().Initialize(target, new Vector3(1.5f, 0, -2f));

        // Body
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
        body.transform.SetParent(panther.transform);
        body.transform.localScale = new Vector3(0.8f, 0.6f, 1.8f);
        body.GetComponent<Renderer>().material = _robotBodyMat;

        // Head
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);
        head.transform.SetParent(panther.transform);
        head.transform.localPosition = new Vector3(0, 0.3f, 1.1f);
        head.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        head.GetComponent<Renderer>().material = _robotBodyMat;

        // Glowing Eyes
        GameObject eyeL = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        eyeL.transform.SetParent(head.transform); eyeL.transform.localPosition = new Vector3(-0.2f, 0.1f, 0.5f); eyeL.transform.localScale = Vector3.one * 0.2f;
        eyeL.GetComponent<Renderer>().material = _robotGlowMat;
        GameObject eyeR = Instantiate(eyeL, head.transform); eyeR.transform.localPosition = new Vector3(0.2f, 0.1f, 0.5f);
        
        // Paw Sparks
        GameObject pawSparks = new GameObject("Paw_Sparks");
        pawSparks.transform.SetParent(panther.transform);
        pawSparks.transform.localPosition = new Vector3(0, -0.3f, 0.9f);
        ParticleSystem ps = pawSparks.AddComponent<ParticleSystem>();
        var main = ps.main; main.startLifetime = 0.3f; main.startSpeed = 1f; main.startSize = 0.05f; main.maxParticles = 100;
        var emission = ps.emission; emission.rateOverTime = 50;
        var shape = ps.shape; shape.shapeType = ParticleSystemShapeType.Cone; shape.angle = 40; shape.radius = 0.1f;
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = _blueTreeMat;
    }

    private void CreateEnvironment()
    {
        GameObject environmentRoot = new GameObject("Procedural_Environment");
        CreateRailway(environmentRoot.transform, 100, 4.0f);
        CreateCityscape(environmentRoot.transform, 50, 150f);
        CreateFloatingVehicles(environmentRoot.transform);
    }
    
    private void CreateRailway(Transform parent, int length, float spacing)
    {
        // Create a more detailed, elevated, double railway
        for (int i = -length; i < length; i++)
        {
            Vector3 pos = new Vector3(0, 0, i * spacing);
            // Support Pillar
            GameObject pillar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pillar.transform.SetParent(parent);
            pillar.transform.position = new Vector3(0, -10, pos.z);
            pillar.transform.localScale = new Vector3(2, 10, 2);
            pillar.GetComponent<Renderer>().material = _buildingMat;

            // Left and Right Tracks
            CreateSingleTrack(parent, pos + new Vector3(-3, 0, 0));
            CreateSingleTrack(parent, pos + new Vector3(3, 0, 0));
        }
    }
    
    private void CreateSingleTrack(Transform parent, Vector3 position)
    {
        // Sleepers
        GameObject sleeper = GameObject.CreatePrimitive(PrimitiveType.Cube);
        sleeper.transform.SetParent(parent);
        sleeper.transform.position = position;
        sleeper.transform.localScale = new Vector3(2.5f, 0.1f, 4f);
        sleeper.GetComponent<Renderer>().material = _railMat;
        // Rails
        GameObject railL = GameObject.CreatePrimitive(PrimitiveType.Cube);
        railL.transform.SetParent(parent); railL.transform.position = position + new Vector3(-0.8f, 0.15f, 0);
        railL.transform.localScale = new Vector3(0.1f, 0.2f, 4f);
        railL.GetComponent<Renderer>().material = _railMat;
        GameObject railR = Instantiate(railL, parent); railR.transform.position = position + new Vector3(0.8f, 0.15f, 0);
    }

    private void CreateCityscape(Transform parent, int buildingCount, float range)
    {
        for (int i = 0; i < buildingCount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-range, range), 0, Random.Range(20, range));
            if (Mathf.Abs(pos.x) < 15) pos.x = Mathf.Sign(pos.x) * 15; // Keep clear of tracks

            float height = Random.Range(50, 200);
            GameObject building = new GameObject($"Building_{i}");
            building.transform.SetParent(parent); building.transform.position = pos;

            GameObject core = GameObject.CreatePrimitive(PrimitiveType.Cube);
            core.transform.SetParent(building.transform); core.transform.localPosition = new Vector3(0, height/2, 0);
            core.transform.localScale = new Vector3(Random.Range(10, 30), height, Random.Range(10, 30));
            core.GetComponent<Renderer>().material = _buildingMat;
            
            // Add emissive windows
            if(Random.value > 0.3f)
            {
                GameObject windows = GameObject.CreatePrimitive(PrimitiveType.Plane);
                windows.transform.SetParent(core.transform);
                windows.transform.localPosition = new Vector3(0, 0, -0.51f);
                windows.transform.localScale = Vector3.one * 0.8f;
                windows.GetComponent<Renderer>().material = _windowMat;
            }
        }
    }
    
    private void CreateFloatingVehicles(Transform parent)
    {
        // Blimp
        GameObject blimp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        blimp.transform.SetParent(parent);
        blimp.transform.position = new Vector3(-80, 80, 100);
        blimp.transform.localScale = new Vector3(30, 10, 10);
        blimp.GetComponent<Renderer>().material = _railMat;
        
        // Drones
        GameObject drone = GameObject.CreatePrimitive(PrimitiveType.Cube);
        drone.transform.SetParent(parent);
        drone.transform.position = new Vector3(20, 30, 50);
        drone.transform.localScale = new Vector3(2, 0.5f, 2);
        drone.GetComponent<Renderer>().material = _robotBodyMat;
    }

    private void CreateVFX()
    {
        // Ambient floating particles
        GameObject particles = new GameObject("Ambient_Particles");
        ParticleSystem ps = particles.AddComponent<ParticleSystem>();
        var main = ps.main; main.startLifetime = 5f; main.startSpeed = 0.5f; main.startSize = 0.1f; main.maxParticles = 500;
        var emission = ps.emission; emission.rateOverTime = 30;
        var shape = ps.shape; shape.shapeType = ParticleSystemShapeType.Box; shape.scale = new Vector3(100, 50, 200);
        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = _coinMat;
    }

    private void CreateUI()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            var canvasGO = new GameObject("Procedural_Canvas");
            canvas = canvasGO.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasGO.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasGO.AddComponent<GraphicRaycaster>();
        }

        // Create Score Panel
        if (scoreText == null) {
            var scorePanel = CreateUIPanel("ScorePanel", canvas.transform, new Vector2(0.05f, 0.9f), new Vector2(0.4f, 0.95f));
            scoreText = CreateUIText("ScoreText", scorePanel.transform, "SCORE: 1,452,780");
        }

        // Create Combo Panel
        if (comboText == null)
        {
            var comboPanel = CreateUIPanel("ComboPanel", canvas.transform, new Vector2(0.8f, 0.82f), new Vector2(0.95f, 0.95f));
            comboText = CreateUIText("ComboText", comboPanel.transform, "COMBO: x55");
            comboText.rectTransform.anchorMin = new Vector2(0.1f, 0.6f);
            comboText.rectTransform.anchorMax = new Vector2(0.9f, 0.9f);
            // Combo Bar
            var barBG = new GameObject("ComboBarBG").AddComponent<Image>();
            barBG.transform.SetParent(comboPanel.transform); barBG.rectTransform.anchorMin = new Vector2(0.1f, 0.1f); barBG.rectTransform.anchorMax = new Vector2(0.9f, 0.3f);
            barBG.rectTransform.offsetMin = barBG.rectTransform.offsetMax = Vector2.zero;
            barBG.color = new Color(0,0,0,0.5f);
            comboProgressBar = new GameObject("ComboBarFill").AddComponent<Image>();
            comboProgressBar.transform.SetParent(barBG.transform); comboProgressBar.rectTransform.anchorMin = Vector2.zero; comboProgressBar.rectTransform.anchorMax = new Vector2(0.8f, 1f);
            comboProgressBar.rectTransform.offsetMin = comboProgressBar.rectTransform.offsetMax = Vector2.zero;
            comboProgressBar.material = _uiBarMat;
        }
    }
    
    private GameObject CreateUIPanel(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax)
    {
        var panel = new GameObject(name).AddComponent<Image>();
        panel.transform.SetParent(parent);
        panel.rectTransform.anchorMin = anchorMin; panel.rectTransform.anchorMax = anchorMax;
        panel.rectTransform.offsetMin = panel.rectTransform.offsetMax = Vector2.zero;
        panel.material = _uiPanelMat;
        return panel.gameObject;
    }

    private Text CreateUIText(string name, Transform parent, string content)
    {
        var textGO = new GameObject(name);
        textGO.transform.SetParent(parent);
        Text text = textGO.AddComponent<Text>();
        text.font = Font.CreateDynamicFontFromOSFont("Arial", 28);
        text.fontStyle = FontStyle.Bold;
        text.color = new Color(0.8f, 0.9f, 1f, 0.9f);
        text.text = content;
        text.rectTransform.anchorMin = new Vector2(0.1f, 0.1f);
        text.rectTransform.anchorMax = new Vector2(0.9f, 0.9f);
        text.rectTransform.offsetMin = text.rectTransform.offsetMax = Vector2.zero;
        return text;
    }

    #endregion
}

// Enhanced helper component for the robot dog.
public class FollowPlayer : MonoBehaviour
{
    private Transform _player;
    private Vector3 _offset;

    public void Initialize(Transform player, Vector3 offset)
    {
        _player = player;
        _offset = offset;
    }

    void Start()
    {
        if (_player == null) // Fallback if not initialized
        {
            _player = GameObject.FindGameObjectWithTag("Player")?.transform;
            _offset = new Vector3(1.5f, 0.5f, -3f);
        }
    }

    void LateUpdate()
    {
        if (_player != null)
        {
            transform.position = Vector3.Lerp(transform.position, _player.position + _player.transform.TransformDirection(_offset), Time.deltaTime * 7f);
            Quaternion targetRotation = Quaternion.LookRotation(_player.forward, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
