using System.Collections;
using System.Collections.Generic;
using Lucine.UISystem;
using UnityEngine;

namespace Lucine.UISystem
{
    public abstract class UIWindow : UIWindow<UIWindowParameters> { }

    public abstract class UIWindow<TParameters> : UIScreenController<TParameters>, IUIWindowController where TParameters : IUIWindowParameters
    {
        public bool HideWhenFocusLost
        {
            get => Parameters.HideWhenFocusLost;
        }

        public bool IsPopup
        {
            get => Parameters.IsPopup;
        }
        
        public virtual void UI_Close()
        {
            OnCloseRequest(this);
        }
    }
}