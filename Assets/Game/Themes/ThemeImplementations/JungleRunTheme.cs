using UnityEngine;

[CreateAssetMenu(fileName = "JungleRunTheme", menuName = "Themes/JungleRun Theme")]
public class JungleRunTheme : ThemeData
{
    [SerializeField] private string themeName;
    [SerializeField] private GameObject[] environmentPrefabs;
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject enemyChaserPrefab;
    [SerializeField] private Material skybox;
    [SerializeField] private Light lighting;
    [SerializeField] private Color uiAccentColor;
    [SerializeField] private AudioClip music;

    public override string Name => themeName;
    public override GameObject[] EnvironmentPrefabs => environmentPrefabs;
    public override GameObject[] ObstaclePrefabs => obstaclePrefabs;
    public override GameObject CoinPrefab => coinPrefab;
    public override GameObject EnemyChaserPrefab => enemyChaserPrefab;
    public override Material Skybox => skybox;
    public override Light Lighting => lighting;
    public override Color UIAccentColor => uiAccentColor;
    public override AudioClip Music => music;
}
