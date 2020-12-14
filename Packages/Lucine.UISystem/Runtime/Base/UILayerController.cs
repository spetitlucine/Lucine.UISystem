using System.Collections.Generic;
using UnityEngine;

namespace Lucine.UISystem
{
    /// <summary>
    /// The layer class. The layer is responsible of all child screens, we should pass from the layer to show or hide screens
    /// The class is inherited with panel layers and windows layers
    /// Windows layer may have some features that panels don't have
    /// It is not implemented yet but we can think about handling history of windows when closing one window to show the previously opened one
    /// Layers are templated with the type of screen they handles
    /// One of the main role of this class is to make association of a screen with its id and have corresponding functions
    /// </summary>
    /// <typeparam name="TScreenController">The type of screen handled by the layer</typeparam>
    public abstract class UILayerController<TScreenController> : MonoBehaviour where TScreenController : IUIScreenController
    {
        // layers contains reference to handled screens from their screen ids
        // it is filled upon initialization
        protected Dictionary<string, TScreenController> m_registeredScreens;

        /// <summary>
        /// The initialization function. This is called to initialize a layer
        /// all child screen in the layer will be registred
        /// On initialization all screen are hidden
        /// </summary>
        public virtual void Initialize()
        {
            m_registeredScreens = new Dictionary<string, TScreenController>();

            TScreenController[] controllers = GetComponentsInChildren<TScreenController>();
            foreach (TScreenController s in controllers)
            {
                RegisterScreen(s.ScreenId, s);
                s.Hide(false);
            }
        }
        
        /// <summary>
        /// This function is called to show a screen
        /// It should be implemented in the inherited layer
        /// </summary>
        /// <param name="screen">The screen controller to show</param>
        protected abstract void ShowScreen(TScreenController screen);

        /// <summary>
        /// This function is used to show a screen but giving it some parameters (the parameters used when defining the screen)
        /// It should be implemented in the inherited layer
        /// </summary>
        /// <param name="screen">The screen to show</param>
        /// <param name="parameters">The wanted parameters</param>
        /// <typeparam name="TParameters">The type of the parameters</typeparam>
        public abstract void ShowScreen<TParameters>(TScreenController screen, TParameters parameters)
            where TParameters : IUIScreenParameters;

        /// <summary>
        /// This function is called to hide a screen
        /// It should be implemented in the inherited layer
        /// </summary>
        /// <param name="screen"></param>
        protected abstract void HideScreen(TScreenController screen);

        /// <summary>
        /// Show a screen using it's id. It will look in registred screen to find the associated controller
        /// </summary>
        /// <param name="screenId">The id to show</param>
        public void ShowScreenById(string screenId)
        {
            if (m_registeredScreens.TryGetValue(screenId, out var ctl))
            {
                ShowScreen(ctl);
            }
            else
            {
                Debug.LogError("[UILayerController] Screen ID " + screenId + " not registered to this layer!");
            }
        }
        
        /// <summary>
        /// Hide a screen from its id
        /// </summary>
        /// <param name="screenId">The id of the screen to hide. It will look in registred screens the associated controller</param>
        public void HideScreenById(string screenId)
        {
            if (m_registeredScreens.TryGetValue(screenId, out var ctl))
            {
                HideScreen(ctl);
            }
            else
            {
                Debug.LogError("[UILayerController] Could not hide Screen ID " + screenId + " as it is not registered to this layer!");
            }
        }
        
        /// <summary>
        /// Register a screen in the layer
        /// If asked screen is already registred, just throw a log error
        /// Screens can be registred only once
        /// </summary>
        /// <param name="screenId">The screen id to register</param>
        /// <param name="controller">The controller associated to the screen</param>
        private void RegisterScreen(string screenId, TScreenController controller)
        {
            if (!m_registeredScreens.ContainsKey(screenId))
            {
                ProcessScreenRegister(screenId, controller);
            }
            else
            {
                Debug.LogError("[UILayerController] Screen controller already registered for id: " + screenId);
            }
        }

        /// <summary>
        /// Pending Unregister scren function
        /// </summary>
        /// <param name="screenId">The screen to unregister</param>
        /// <param name="controller">The associated controller</param>
        private void UnregisterScreen(string screenId, TScreenController controller)
        {
            if (m_registeredScreens.ContainsKey(screenId))
            {
                ProcessScreenUnregister(screenId, controller);
            }
            else
            {
                Debug.LogError("[UILayerController] Screen controller not registered for id: " + screenId);
            }
        }
        

        /// <summary>
        /// This function may be used to know if a screen is registred
        /// </summary>
        /// <param name="screenId">id of the screen to check</param>
        /// <returns></returns>
        public bool IsScreenRegistered(string screenId)
        {
            return m_registeredScreens.ContainsKey(screenId);
        }
        

        /// <summary>
        /// really registrer the screen
        /// This function also register to screen destroy event to be inform if the registred screen is destroyed
        /// </summary>
        /// <param name="screenId">The screen id to register</param>
        /// <param name="controller">The associated controller</param>
        protected virtual void ProcessScreenRegister(string screenId, TScreenController controller)
        {
            controller.ScreenId = screenId;
            m_registeredScreens.Add(screenId, controller);
            // register to OnScreenDestroy event
            controller.OnScreenDestroyed += OnScreenDestroyed;
        }
        
        /// <summary>
        /// Really unregister a screen and remove listener on screen destroyed callback
        /// </summary>
        /// <param name="screenId">The screen Id</param>
        /// <param name="controller">The screen controller</param>
        protected virtual void ProcessScreenUnregister(string screenId, TScreenController controller)
        {
            if (controller != null) controller.OnScreenDestroyed -= OnScreenDestroyed;
            m_registeredScreens.Remove(screenId);
        }
        
        /// <summary>
        /// Hide all screens of the layer animating them or not depending of parameter
        /// </summary>
        /// <param name="shouldAnimateWhenHiding">Should animate closing</param>
        public virtual void HideAll(bool shouldAnimateWhenHiding = true)
        {
            foreach (var screen in m_registeredScreens) {
                screen.Value.Hide(shouldAnimateWhenHiding);
            }
        }
        
        /// <summary>
        /// Callback when a registred screen is destroyed
        /// </summary>
        /// <param name="screen"></param>
        private void OnScreenDestroyed(IUIScreenController screen)
        {
            if (!string.IsNullOrEmpty(screen.ScreenId) && m_registeredScreens.ContainsKey(screen.ScreenId))
            {
                UnregisterScreen(screen.ScreenId, (TScreenController) screen);
            }
        }
        
        /// <summary>
        /// Function that check if a screen is visible using the id
        /// </summary>
        /// <param name="screenId"></param>
        /// <returns></returns>
        protected bool IsScreenVisibleById(string screenId)
        {
            if (m_registeredScreens.TryGetValue(screenId, out var ctl))
            {
                return ctl.IsVisible;
            }
            else
            {
                Debug.LogError("[UILayerController] Could not indicates visibility Screen ID " + screenId + " as it is not registered to this layer!");
                return false;
            }
        }
    }
}