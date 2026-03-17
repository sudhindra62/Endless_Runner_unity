
using UnityEngine;

namespace EndlessRunner.Character
{
    public enum SkinUnlockType { Coins, Gems, Premium }

    [CreateAssetMenu(fileName = "NewCharacterSkin", menuName = "EndlessRunner/Character Skin")]
    public class CharacterSkin : ScriptableObject
    {
        public string characterName;
        public GameObject gamePrefab; // The prefab with gameplay components
        public GameObject previewPrefab; // The prefab for the 360 previewer
        
        [Header("Unlock Criteria")]
        public SkinUnlockType unlockType;
        public int price;
        
        [Header("Animations")]
        public AnimationClip idleAnimation;
        public AnimationClip runAnimation;
        public AnimationClip slideAnimation;
        public AnimationClip jumpAnimation;
    }
}
