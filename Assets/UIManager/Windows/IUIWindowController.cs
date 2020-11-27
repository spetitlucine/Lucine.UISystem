using System.Collections;
using System.Collections.Generic;
using Lucine.UISystem;
using UnityEngine;

namespace Lucine.UISystem
{
    public interface IUIWindowController : IUIScreenController
    {
        bool HideWhenFocusLost { get; }
        bool IsPopup { get; }
    }
}
