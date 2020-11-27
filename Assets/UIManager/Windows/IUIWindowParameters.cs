using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lucine.UISystem
{
    public interface IUIWindowParameters : IUIScreenParameters
    {
        bool IsPopup { get; set; } 
        bool HideWhenFocusLost { get; set; } 
    }
}