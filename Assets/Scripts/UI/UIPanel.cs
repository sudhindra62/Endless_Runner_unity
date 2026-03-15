
using UnityEngine;
using EndlessRunner.Managers;

namespace EndlessRunner.UI
{
    /// <summary>
    /// An abstract base class for all UI panels. Provides foundational Show/Hide logic
    /// and a standardized interface for the UIManager.
    /// </summary>
    public abstract class UIPanel : MonoBehaviour
    {
        protected UIManager _uiManager;

        /// <summary>
        /// Initializes the panel with a reference to the UIManager.
        /// </summary>
        public virtual void Setup(UIManager uiManager)
        {
            _uiManager = uiManager;
        }

        /// <summary>
        /// Activates the panel's GameObject.
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Deactivates the panel's GameObject.
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
