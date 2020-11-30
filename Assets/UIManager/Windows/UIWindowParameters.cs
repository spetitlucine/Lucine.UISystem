using UnityEngine;

namespace Lucine.UISystem
{
    /// <summary>
    /// Base class for WindowParameters
    /// Implement the IUIWindowParameters
    /// Default parameters for windows : hideWhenFocusLost (default true), isPopup (default false)
    /// class must be serializable to show in inspector
    /// </summary>
    [System.Serializable] 
    public class UIWindowParameters : IUIWindowParameters
    {
        [SerializeField] 
        protected bool m_HideWhenFocusLost = true;
        public bool HideWhenFocusLost
        {
            get => m_HideWhenFocusLost;
            set => m_HideWhenFocusLost = value;
        }

        [SerializeField]
        protected bool m_IsPopup = false;
        public bool IsPopup
        {
            get => m_IsPopup;
            set => m_IsPopup = value;
        }

        public UIWindowParameters()
        {
            m_HideWhenFocusLost = true;
            m_IsPopup = false;
        }

    }
}

