
using UnityEngine;


// Removed namespace EndlessRunner.Core
    /// <summary>
    /// A generic, thread-safe Singleton base class for any MonoBehaviour.
    /// Ensures that only one instance of the Singleton exists in the scene.
    /// Automatically persists across scene loads.
    /// </summary>
    /// <typeparam name="T">The type of the singleton.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();

        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        // Check if an instance already exists in the scene
                        _instance = FindFirstObjectByType<T>();

                        if (_instance == null)
                        {
                            // If not, create a new GameObject and add the component
                            GameObject singletonObject = new GameObject(typeof(T).Name);
                            _instance = singletonObject.AddComponent<T>();
                        }
                    }
                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject); // Persist across scenes
            }
            else if (_instance != this)
            {
                Debug.LogWarning($"SINGLETON: An instance of {typeof(T).Name} already exists. Destroying duplicate.");
                Destroy(gameObject); // Enforce only one instance
            }
        }
    }
