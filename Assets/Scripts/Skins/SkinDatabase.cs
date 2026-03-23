using UnityEngine;
using System.Collections.Generic;


    [CreateAssetMenu(fileName = "SkinDatabase", menuName = "EndlessRunner/SkinDatabase")]
    public class SkinDatabase : ScriptableObject
    {
        [System.Serializable]
        public class SkinCatalog
        {
            public SkinData[] skins;
        }

        public List<SkinData> skins;
        public SkinCatalog skinCatalog => new SkinCatalog { skins = skins != null ? skins.ToArray() : new SkinData[0] };

        public List<SkinData> GetAllSkins()
        {
            return skins;
        }
    }

