using UnityEngine;

namespace MyGame.Managers
{
    public class PersistentDataManager : MonoBehaviour
    {
        public static PersistentDataManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SaveData(string key, object data)
        {
            string jsonData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, jsonData);
            PlayerPrefs.Save();
        }

        public T LoadData<T>(string key)
        {
            string jsonData = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<T>(jsonData);
        }
    }
}
