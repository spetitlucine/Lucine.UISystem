namespace Lucine.UISystem
{
    /// <summary>
    /// Interface to implement for windows screen controllers
    /// It is related to UIWindowParameters
    /// </summary>
    public interface IUIWindowController : IUIScreenController
    {
        /// <summary>
        /// to know if the window should be hidden when loosing foreground (another window on top)
        /// </summary>
        bool HideWhenForegroundLost { get; }
        
        /// <summary>
        /// to know if the window is a popup
        /// </summary>
        bool IsPopup { get; }
    }
}
