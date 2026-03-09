
using UnityEngine;

namespace Core
{
    /// <summary>
    /// A generic base class for creating singletons from MonoBehaviours.
    /// This ensures that only one instance of the class exists, providing a global access point.
    /// It handles persistence across scene loads and prevents the creation of "ghost" objects in the editor.
    /// </summary>
    /// <typeparam name="T">The type of the singleton class, which must inherit from MonoBehaviour.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // The single, static instance of the singleton.
        private static T _instance;

        // A lock object to ensure thread safety during instance access.
        private static readonly object _lock = new object();

        // Flag to prevent re-creation of the instance during application shutdown.
        private static bool _isApplicationQuitting = false;

        /// <summary>
        /// Gets the singleton instance. If it doesn't exist, it will be found or created.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (_isApplicationQuitting)
                {
                    Debug.LogWarning($"[Singleton] Instance of '{typeof(T)}' will not be created because the application is quitting.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        // Try to find an existing instance in the scene.
                        _instance = FindObjectOfType<T>();

                        // If no instance exists, create a new one.
                        if (_instance == null)
                        {
                            var singletonObject = new GameObject();
                            _instance = singletonObject.AddComponent<T>();
                            singletonObject.name = $"{typeof(T)} (Singleton)";
                        }
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// This is the primary mechanism for enforcing the singleton pattern.
        /// It's a virtual method to allow derived classes to extend its behavior.
        /// </summary>
        protected virtual void Awake()
        {
            if (_instance == null)
            {
                // If this is the first instance, make it the singleton.
                _instance = this as T;
                
                // Mark this object to not be destroyed when loading new scenes.
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                // If another instance already exists, destroy this one.
                Debug.LogWarning($"[Singleton] Duplicate instance of '{typeof(T)}' found. Destroying the duplicate.");
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// OnDestroy is called when the MonoBehaviour will be destroyed.
        /// We set a flag here to prevent any further access to the singleton instance.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _isApplicationQuitting = true;
            }
        }
    }
}
