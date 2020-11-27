using System.Collections;
using System.Collections.Generic;
using Lucine.Helpers;
using Lucine.UISystem;
using UnityEngine;

public class DemoPanel : UIPanel
{
    public class OnPanelClosedEvent : Event<DemoPanel> { }

    public void OnClosePanel()
    {
        Events.Instance.Get<OnPanelClosedEvent>().Dispatch(this);
        Hide();
    }
}
