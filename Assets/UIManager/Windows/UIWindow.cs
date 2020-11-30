namespace Lucine.UISystem
{
    /// <summary>
    /// A basic window with default windows parameters
    /// </summary>
    public abstract class UIWindow : UIWindow<UIWindowParameters> { }

    /// <summary>
    /// A window with another type of window parameters
    /// </summary>
    /// <typeparam name="TParameters"></typeparam>
    public abstract class UIWindow<TParameters> : UIScreenController<TParameters>, IUIWindowController where TParameters : IUIWindowParameters
    {
        /// <summary>
        ///  implement IUIWindow interface
        ///  HideWhenForeground lost. return 
        /// </summary>
        public bool HideWhenForegroundLost => Parameters.HideWhenFocusLost;

        /// <summary>
        /// Implement IsPopup 
        /// </summary>
        public bool IsPopup => Parameters.IsPopup;
        
        /// <summary>
        /// This function is aim to be bind to close button of the interface in Unity Button. It is an helper always present when a window has a close button
        /// It send an OnCloseRequest to listeners
        /// </summary>
        public virtual void UnityClose()
        {
            OnCloseRequest(this);
        }
        
        /// <summary>
        /// By overriding the AdjustHierarchyOnShow, UIWindows are always put to top when opening
        /// It is the role of the developper to make sure that a window does not open when a popup is displayed.
        /// </summary>
        protected override void AdjustHierarchyOnShow()
        {
            transform.SetAsLastSibling();
        }

    }
}