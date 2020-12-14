using Lucine.Helpers;
using Lucine.UISystem;

public class DemoPanel : UIPanel
{
    // define a OnPanelCloseEvent with one parameter DemoPanel type
    public class OnPanelClosedEvent : Event<DemoPanel> { }

    /// <summary>
    /// Called by unity when the close panel button is pressed
    /// </summary>
    public void OnClosePanel()
    {
        // dispatch the event to all listener giving in parameter the panel which is closed
        Events.Instance.TypeOf<OnPanelClosedEvent>().Dispatch(this);
        
        // hide the panel
        Hide();
    }
}
