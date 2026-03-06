
using UnityEngine;

namespace Managers
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }
        
        public GameData GameData { get; private set; }

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

        public void LoadGameData()
        {
            // In a real project, this would load from a file
            GameData = new GameData(); 
            
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.LoadCurrencyFromSaveData(GameData);
            }
        }

        public void SaveGameData()
        {
            if (CurrencyManager.Instance != null)
            {
                CurrencyManager.Instance.PopulateSaveData(GameData);
            }
            
            // In a real project, this would save to a file
        }
    }
}
