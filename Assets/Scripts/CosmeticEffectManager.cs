
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace EndlessRunner.Cosmetics
{
    /// <summary>
    /// Manages the database of cosmetic effects and the currently equipped effect for the player.
    /// This system is the central authority for cosmetic visuals.
    /// Fully re-architected by the Supreme Guardian Architect v13.
    /// </summary>
    public class CosmeticEffectManager : Singleton<CosmeticEffectManager>
    {
        [Header("Configuration")]
        [Tooltip("A list of all possible cosmetic effects available in the game.")]
        [SerializeField] private List<CosmeticEffectData> effectDatabase = new List<CosmeticEffectData>();

        [Tooltip("The transform to which equipped effects will be parented. If null, will default to this manager's transform.")]
        [SerializeField] private Transform effectParent;

        public CosmeticEffectData CurrentlyEquippedEffect { get; private set; }

        private GameObject _currentEffectInstance;
        private const string EQUIPPED_EFFECT_SAVE_KEY = "EquippedCosmeticEffectID";

        protected override void Awake()
        {
            base.Awake();
            if (effectParent == null)
            {
                effectParent = transform;
            }
        }

        private void Start()
        {
            LoadEquippedEffect();
        }

        public void EquipEffect(string effectID)
        {
            UnequipCurrentEffect();

            if (string.IsNullOrEmpty(effectID))
            {
                Debug.Log("Guardian Architect: EquipEffect called with null or empty ID. Effect has been unequipped.");
                SaveEquippedEffect(null);
                return;
            }

            CosmeticEffectData effectToEquip = GetEffectByID(effectID);

            if (effectToEquip == null)
            {
                Debug.LogError($"Guardian Architect FATAL_ERROR: Cannot equip effect. ID '{effectID}' not found in the database.");
                return;
            }

            if (!CosmeticInventoryManager.Instance.IsEffectUnlocked(effectID))
            {
                Debug.LogWarning($"Guardian Architect Warning: Cannot equip effect '{effectID}'. It is not unlocked.");
                return;
            }

            if (effectToEquip.EffectPrefab != null)
            {
                _currentEffectInstance = Instantiate(effectToEquip.EffectPrefab, effectParent.position, effectParent.rotation, effectParent);
            }

            CurrentlyEquippedEffect = effectToEquip;
            SaveEquippedEffect(effectID);
            Debug.Log($"<color=lime>Guardian Architect: Cosmetic Effect '{effectToEquip.DisplayName}' equipped.</color>");
        }

        public void UnequipCurrentEffect()
        {
            if (_currentEffectInstance != null)
            {
                Destroy(_currentEffectInstance);
                _currentEffectInstance = null;
            }
            CurrentlyEquippedEffect = null;
            SaveEquippedEffect(null);
        }

        public CosmeticEffectData GetEffectByID(string effectID)
        {
            return effectDatabase.FirstOrDefault(e => e.EffectID == effectID);
        }

        public IEnumerable<CosmeticEffectData> GetAllEffects()
        {
            return effectDatabase;
        }

        private void SaveEquippedEffect(string effectID)
        {
            if (string.IsNullOrEmpty(effectID))
            {
                PlayerPrefs.DeleteKey(EQUIPPED_EFFECT_SAVE_KEY);
            }
            else
            {
                PlayerPrefs.SetString(EQUIPPED_EFFECT_SAVE_KEY, effectID);
            }
            PlayerPrefs.Save();
        }

        private void LoadEquippedEffect()
        {
            string equippedID = PlayerPrefs.GetString(EQUIPPED_EFFECT_SAVE_KEY, null);
            if (!string.IsNullOrEmpty(equippedID))
            {
                if (CosmeticInventoryManager.Instance.IsEffectUnlocked(equippedID))
                {
                    EquipEffect(equippedID);
                }
                else
                {
                    Debug.LogWarning($"Guardian Architect Warning: Saved effect '{equippedID}' is no longer unlocked. Unequipping.");
                    UnequipCurrentEffect();
                }
            }
        }
    }
}
