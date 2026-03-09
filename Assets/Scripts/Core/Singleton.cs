using UnityEngine;

namespace Core
{
    /// <summary>
    /// A generic, thread-safe singleton pattern implementation for MonoBehaviour components.
    /// Ensures that only one instance of a singleton exists in the scene.
    /// Logic consolidated and fortified by Supreme Guardian Architect v12.
    /// API updated to Unity 6 standards by OMNI_GUARDIAN_ARCHITECT_v12_SUPREME.
    /// </summary>
    /// <typeparam name="T">The type of the singleton component.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object _lock = new object();
        private static bool _applicationIsQuitting = false;

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning($"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Won't create again - returning null.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindFirstObjectByType<T>();

                        if (FindObjectsByType<T>(FindObjectsSortMode.None).Length > 1)
                        {
                            Debug.LogError("[Singleton] Something went really wrong - there's more than one singleton! Reopening the scene might fix it.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = "(singleton) " + typeof(T).ToString();

                            DontDestroyOnLoad(singleton);

                            Debug.Log($"[Singleton] An instance of {typeof(T)} is needed in the scene, so '{singleton}' was created with DontDestroyOnLoad.");
                        }
                        else
                        {
                            Debug.Log($"[Singleton] Using instance already created: {_instance.gameObject.name}");
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
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.Log($"[Singleton] An instance of {typeof(T)} already exists. Destroying this duplicate.");
                Destroy(gameObject);
            }
        }

        public void OnDestroy()
        {
            _applicationIsQuitting = true;
        }
    }
}
