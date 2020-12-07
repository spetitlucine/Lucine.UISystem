using UnityEngine;

namespace Lucine.UISystem
{
    /// <summary>
    /// Base class for WindowParameters
    /// Implement the IUIWindowParameters
    /// Default parameters for windows : hideWhenForegroundLost (default true), isPopup (default false)
    /// class must be serializable to show in inspector
    /// </summary>
    [System.Serializable] 
    public class UIWindowParameters : IUIWindowParameters
    {
        [SerializeField] 
        protected bool m_HideWhenForegroundLost = true;
        public bool HideWhenForegroundLost
        {
            get => m_HideWhenForegroundLost;
            set => m_HideWhenForegroundLost = value;
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
            m_HideWhenForegroundLost = true;
            m_IsPopup = false;
        }

    }
}

