using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lucine.UISystem
{
    [System.Serializable] 
    public class UIWindowParameters : IUIWindowParameters
    {
        [SerializeField] 
        protected bool hideWhenFocusLost = true;

        [SerializeField]
        protected bool isPopup = false;

        public UIWindowParameters()
        {
            hideWhenFocusLost = true;
            isPopup = false;
        }
        public bool HideWhenFocusLost
        {
            get => hideWhenFocusLost;
            set => hideWhenFocusLost = value;
        }

        public bool IsPopup
        {
            get => isPopup;
            set => isPopup = value;
        }
    }
}

