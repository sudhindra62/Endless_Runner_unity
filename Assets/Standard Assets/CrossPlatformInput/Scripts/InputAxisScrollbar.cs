using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput
{
    [RequireComponent(typeof(UnityEngine.UI.Scrollbar))]
    public class InputAxisScrollbar : MonoBehaviour
    {
        public string axis;
        private UnityEngine.UI.Scrollbar m_Scrollbar;


        void OnEnable()
        {
            m_Scrollbar = GetComponent<UnityEngine.UI.Scrollbar>();
        }


        void Update()
        {
        }


        public void HandleInput(float value)
        {
            CrossPlatformInputManager.SetAxis(axis, (value*2f) - 1f);
        }
    }
}
