using UnityEngine;

namespace Skins
{
    public class SkinDatabase : MonoBehaviour
    {
        public SkinCatalog skinCatalog;

        private void Awake()
        {
            ServiceLocator.Register<SkinDatabase>(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.Unregister<SkinDatabase>();
        }

        public SkinData GetSkin(string skinName)
        {
            return skinCatalog.skins.Find(s => s.skinName == skinName);
        }

        public SkinData[] GetAllSkins()
        {
            return skinCatalog.skins.ToArray();
        }
    }
}
