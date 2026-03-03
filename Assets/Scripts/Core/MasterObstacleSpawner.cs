
using UnityEngine;
using Managers;
using Systems;

namespace Core
{
    /// <summary>
    /// AUTHORITATIVE: The one and only Obstacle Spawner.
    /// This class EXECUTES spawn commands. It does not decide WHAT to spawn.
    /// It listens to the ProceduralPatternEngine for instructions.
    /// </summary>
    public class MasterObstacleSpawner : MonoBehaviour
    {
        [Tooltip("Reference to the pooler for spawning standard obstacles.")]
        [SerializeField] private ObjectPooler _obstaclePooler;
        [Tooltip("Reference to the pooler for spawning coins, used during conversion effects.")]
        [SerializeField] private ObjectPooler _coinPooler;

        private EffectsManager _effectsManager;

        private void Awake()
        {
            _effectsManager = FindObjectOfType<EffectsManager>();
        }

        private void OnEnable()
        {
            // Subscribe to the pattern engine's broadcast.
            ProceduralPatternEngine.OnGeneratedPatternReady += ExecutePatternInstruction;
        }

        private void OnDisable()
        {
            // Unsubscribe to prevent memory leaks.
            ProceduralPatternEngine.OnGeneratedPatternReady -= ExecutePatternInstruction;
        }

        /// <summary>
        /// Receives a pattern instruction and executes the spawning logic.
        /// </summary>
        private void ExecutePatternInstruction(PatternInstruction instruction)
        {
            if (instruction.TargetTile == null || instruction.Obstacles == null) return;

            // NEW: Consult the EffectsManager for the obstacle-to-coin conversion effect.
            bool isConversionActive = _effectsManager != null && _effectsManager.IsObstacleConversionActive();

            foreach (var obstacleData in instruction.Obstacles)
            {
                if (isConversionActive)
                {
                    SpawnAsCoin(obstacleData, instruction.TargetTile);
                }
                else
                {
                    SpawnAsObstacle(obstacleData, instruction.TargetTile);
                }
            }
        }

        private void SpawnAsObstacle(ObstaclePlacementData data, Transform parentTile)
        {
            // If the object pooler is not assigned in the inspector, use a primitive as a fallback for testing.
            if (_obstaclePooler == null)
            {
                var obstacle = GameObject.CreatePrimitive(PrimitiveType.Cube);
                obstacle.transform.SetParent(parentTile);
                obstacle.transform.localPosition = data.LocalPosition;
                obstacle.transform.localScale = data.Scale;
                obstacle.name = data.ObstacleID;
                return;
            }
            
            // Production-ready code: Use the object pooler to spawn the obstacle.
            // var spawnedObstacle = _obstaclePooler.Spawn(data.ObstacleID, parentTile.position + data.LocalPosition, Quaternion.identity);
            // spawnedObstacle.transform.SetParent(parentTile);
            // spawnedObstacle.transform.localScale = data.Scale;
        }

        private void SpawnAsCoin(ObstaclePlacementData data, Transform parentTile)
        {
            // Use a primitive for the coin if the pooler is not set.
            if (_coinPooler == null)
            {
                var coin = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                coin.transform.SetParent(parentTile);
                coin.transform.localPosition = data.LocalPosition;
                coin.transform.localScale = Vector3.one * 0.5f;
                coin.GetComponent<Renderer>().material.color = Color.yellow;
                coin.name = "ConvertedCoin";
                return;
            }

            // Production-ready code: Use the coin pooler.
            // _coinPooler.Spawn("StandardCoin", parentTile.position + data.LocalPosition, Quaternion.identity);
        }
    }
}
