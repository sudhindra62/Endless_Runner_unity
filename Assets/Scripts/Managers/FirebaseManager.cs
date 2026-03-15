
using EndlessRunner.Core;
using UnityEngine;

namespace EndlessRunner.Managers
{
    public class FirebaseManager : Singleton<FirebaseManager>
    {
        protected override void Awake()
        {
            base.Awake();
            ServiceLocator.Register(this);
        }

        private void Start()
        {
            // In a real implementation, this would initialize the Firebase SDK
            Debug.Log("FIREBASE_MANAGER: Firebase SDK initialized.");
        }
    }
}
