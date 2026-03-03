using UnityEngine;

/// <summary>
/// A generic Singleton base class to ensure only one instance of a manager exists.
/// This pattern simplifies access to managers from anywhere in the code without needing
/// direct references, while also ensuring their uniqueness.
/// </summary>
/// <typeparam name="T">The type of the singleton instance.</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {                    
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {            
            Destroy(this.gameObject);
            return;
        }
        _instance = this as T;
        DontDestroyOnLoad(this.gameObject); // Persist across scenes
    }
}
