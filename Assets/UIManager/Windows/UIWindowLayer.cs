using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lucine.UISystem
{
    /// <summary>
    /// Layer for windows inherited of base UILayerController
    /// It implements interface for UIWindowControllers
    /// Unlike panels layer, the window controller should keep track of transitionning windows to disable interactions during transition
    /// A panel is not aim to be interactable but only display information
    /// We also keep track of current window. Right now it is not used maybe in the future
    /// </summary>
    public class UIWindowLayer : UILayerController<IUIWindowController>
    {
        // the blackbackground processor (there's a default prefab for it to put in windows layer)
        [SerializeField]
        private BlackBackground m_BlackBackground = null;
        
        // a list of all transitionning screens (when this list has 0 element no transitioning is running and interaction can be enabled)
        private List<IUIScreenController> m_screensTransitioning;

        // the current window. Can be get but not set from outside since it the this layer controller that handles it
        public IUIWindowController CurrentWindow { get; private set; }

        // Callback to call when we want to disable or enable interactions
        public Action DisableInteractionRequest;
        public Action EnableInteractionRequest;

        /// <summary>
        /// Function that can indicated if any interaction is running
        /// </summary>
        private bool IsScreenTransitionInProgress => m_screensTransitioning.Count != 0;

        /// <summary>
        /// Windows layer initialization
        /// Creates screen transitionning list and initialize from base
        /// </summary>
        public override void Initialize()
        {
            m_screensTransitioning = new List<IUIScreenController>();
            // hide the blackbackground if exists
            m_BlackBackground?.Hide();
            base.Initialize();
        }

        /// <summary>
        /// Override base layer register screen to add specific callback tracking
        /// Window layer needs to be called when transition are starting and ending because of handles of interaction during transitions
        /// It should be inform too when the unity ui asked to close generic handler (UnityClose)
        /// </summary>
        /// <param name="screenId">The screen id to register</param>
        /// <param name="controller">The associated controller</param>
        protected override void ProcessScreenRegister(string screenId, IUIWindowController controller)
        {
            base.ProcessScreenRegister(screenId, controller);
            controller.OnInTransitionFinished += OnInAnimationFinished;
            controller.OnOutTransitionFinished += OnOutAnimationFinished;
            controller.OnCloseRequest += OnCloseRequestedByWindow;
        }

        /// <summary>
        /// override base layer unregister to remove specific callbacks
        /// </summary>
        /// <param name="screenId">The screen id to unregister</param>
        /// <param name="controller">The associated controller</param>
        protected override void ProcessScreenUnregister(string screenId, IUIWindowController controller)
        {
            base.ProcessScreenUnregister(screenId, controller);
            controller.OnInTransitionFinished -= OnInAnimationFinished;
            controller.OnOutTransitionFinished -= OnOutAnimationFinished;
            controller.OnCloseRequest -= OnCloseRequestedByWindow;
        }

        /// <summary>
        /// Show window controller without parameter
        /// </summary>
        /// <param name="screen">The window controller</param>
        protected override void ShowScreen(IUIWindowController screen)
        {
            ShowScreen<IUIWindowParameters>(screen, null);
        }

        /// <summary>
        /// show window window controller with new given parameters
        /// </summary>
        /// <param name="screen">The window controller</param>
        /// <param name="parameters">The parameters to set</param>
        /// <typeparam name="TParameters">Type of the parameters</typeparam>
        public override void ShowScreen<TParameters>(IUIWindowController screen, TParameters parameters)
        {
            IUIWindowParameters windowParameters = parameters as IUIWindowParameters;

            ProcessShow(screen, windowParameters);
        }

        /// <summary>
        /// Hide the window with given window controller
        /// </summary>
        /// <param name="screen"></param>
        protected override void HideScreen(IUIWindowController screen)
        {
            if (screen == CurrentWindow)
            {
                AddTransition(screen);
                screen.Hide();

                CurrentWindow = null;
            }
            else
            {
                string message = CurrentWindow != null ? CurrentWindow.ScreenId : "current is null";
                Debug.LogError($"[UIWindowLayer] Hide requested on WindowId {screen.ScreenId} but that's not the currently open one ({message})! Ignoring request.");
            }
        }

        /// <summary>
        /// Hide all windows of the layer
        /// </summary>
        /// <param name="shouldAnimateWhenHiding"></param>
        public override void HideAll(bool shouldAnimateWhenHiding = true)
        {
            base.HideAll(shouldAnimateWhenHiding);
            CurrentWindow = null;
        }

        /// <summary>
        /// Really show a window. 
        /// but if it is a popup
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="parameters"></param>
        private void ProcessShow(IUIWindowController screen, IUIWindowParameters parameters)
        {
            //If the current window must be hidden when loosing foreground hide it before opening the new one
            if (CurrentWindow != null && CurrentWindow.HideWhenForegroundLost && !screen.IsPopup)
            {
                CurrentWindow.Hide();
            }

            // if the window is a popup, show the background
            if (screen.IsPopup)
            {
                m_BlackBackground?.Show();
            }

            // register the transition
            AddTransition(screen);

            // show the new window
            screen.Show();

            // make it current one
            CurrentWindow = screen;
        }


        /// <summary>
        /// This callback is called when the UnityUIClose event is launched
        /// </summary>
        /// <param name="screen"></param>
        private void OnCloseRequestedByWindow(IUIScreenController screen)
        {
            HideScreen(screen as IUIWindowController);
        }

        /// <summary>
        /// callback when out animation is done
        /// </summary>
        /// <param name="screen"></param>
        private void OnOutAnimationFinished(IUIScreenController screen)
        {
            // remove from current transition list
            RemoveTransition(screen);
            
            // if window is popup then we can remove the blackscreen
            IUIWindowController window = screen as IUIWindowController;
            if (window != null && window.IsPopup)
            {
                m_BlackBackground?.Hide();
            }
        }

        /// <summary>
        /// called when intransition is done. 
        /// </summary>
        /// <param name="screen"></param>
        private void OnInAnimationFinished(IUIScreenController screen)
        {
            // can remove this one from current transition list
            RemoveTransition(screen);
        }

        /// <summary>
        /// keep track of running transition in order to disable interactions during the transitions
        /// </summary>
        /// <param name="screen"></param>
        private void AddTransition(IUIScreenController screen)
        {
            // if no interaction was in progress ask to disable interactions
            if (!IsScreenTransitionInProgress)
            {
                DisableInteractionRequest?.Invoke();
            }

            // add it to transition list to be able to reactivate interactions
            m_screensTransitioning.Add(screen);
        }

        /// <summary>
        ///  Remove transition from current transition list and reactivate interactions if neeeded
        /// </summary>
        /// <param name="screen"></param>
        private void RemoveTransition(IUIScreenController screen)
        {
            if(m_screensTransitioning.Contains(screen))
                m_screensTransitioning.Remove(screen);
            else
            {
                Debug.Log("[UIWindowLayer : try to remove a transition not started");
            }
            
            // if no more transitions reactivate interaction
            if (!IsScreenTransitionInProgress)
            {
                EnableInteractionRequest?.Invoke();
            }
        }

        /// <summary>
        /// Return if the window is visible or not
        /// </summary>
        /// <param name="windowId"></param>
        /// <returns></returns>
        public bool IsVisible(string windowId)
        {
            return IsScreenVisibleById(windowId);
        }

        /// <summary>
        /// Close the current Window
        /// </summary>
        public void CloseCurrentWindow()
        {
            HideScreen(CurrentWindow);
            CurrentWindow = null;
        }
    }
}