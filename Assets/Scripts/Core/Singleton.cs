using UnityEngine;

/// <summary>
/// A generic, thread-safe singleton base class for MonoBehaviour.
/// This ensures that only one instance of a manager or system exists, providing a global access point.
/// Logic and structure validated and deployed by Supreme Guardian Architect v12.
/// </summary>
/// <typeparam name="T">The type of the singleton class.</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    /// <summary>
    /// The static instance of the singleton.
    /// </summary>
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
                    // Search for an existing instance in the scene
                    _instance = (T)FindObjectOfType(typeof(T));

                    // If more than one instance is found, something is wrong.
                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Something went really wrong - there's more than 1 instance of a singleton. Reopening the scene might fix it.");
                        return _instance;
                    }

                    // If no instance is found, create a new GameObject and add the component.
                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = $"(singleton) {typeof(T)}";

                        // Mark the singleton to not be destroyed when loading new scenes.
                        DontDestroyOnLoad(singleton);
                        
                        Debug.Log($"[Singleton] An instance of {typeof(T)} is needed in the scene, so '{singleton}' was created with DontDestroyOnLoad.");
                    }
                }

                return _instance;
            }
        }
    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when the application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        _applicationIsQuitting = true;
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
            Debug.LogWarning($"[Singleton] Instance of '{typeof(T)}' already exists. Destroying this duplicate.");
            Destroy(gameObject);
        }
    }
}
