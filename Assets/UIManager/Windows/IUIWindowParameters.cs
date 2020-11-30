namespace Lucine.UISystem
{
    /// <summary>
    /// Interface for the window parameters (will be used in inspector)
    /// </summary>
    public interface IUIWindowParameters : IUIScreenParameters
    {
        /// <summary>
        /// Is the window a popup ?
        /// </summary>
        bool IsPopup { get; set; } 
        
        /// <summary>
        /// should window be hidden when loosing foreground (another window on top of it)
        /// </summary>
        bool HideWhenFocusLost { get; set; } 
    }
}