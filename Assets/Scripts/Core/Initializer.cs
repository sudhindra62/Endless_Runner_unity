


using UnityEngine;

    public class Initializer : MonoBehaviour
    {
        private void Awake()
        {
            // The order of initialization can be important. 
            // GameManager should be first as other managers might subscribe to its events.
            gameObject.AddComponent<GameManager>();

            // Add other core managers.
            gameObject.AddComponent<AdManager>();
            gameObject.AddComponent<AnalyticsManager>();
            gameObject.AddComponent<CloudLoggingManager>();
            gameObject.AddComponent<CurrencyManager>();
            gameObject.AddComponent<FirebaseManager>();
            gameObject.AddComponent<ReviveManager>();
            gameObject.AddComponent<ScoreManager>();
            gameObject.AddComponent<TimeManager>();
            gameObject.AddComponent<UIManager>();

            // Add controllers.
            gameObject.AddComponent<GameFlowController>();

            // All singletons are initialized, now the game can proceed.
            // The GameFlowController will set the initial game state.
        }
    }


