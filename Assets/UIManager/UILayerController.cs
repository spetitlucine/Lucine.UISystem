using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lucine.UISystem
{
    public abstract class UILayerController<TScreenController> : MonoBehaviour where TScreenController : IUIScreenController
    {
        protected Dictionary<string, TScreenController> m_registeredScreens;

        public abstract void ShowScreen(TScreenController screen);

        public abstract void ShowScreen<TParameters>(TScreenController screen, TParameters parameters)
            where TParameters : IUIScreenParameters;

        public abstract void HideScreen(TScreenController screen);

        public virtual void Initialize()
        {
            m_registeredScreens = new Dictionary<string, TScreenController>();

            TScreenController[] controllers = GetComponentsInChildren<TScreenController>() as TScreenController[];
            foreach (TScreenController s in controllers)
            {
                RegisterScreen(s.ScreenId, s);
                s.Hide(false);
            }
        }

        public virtual void ReparentScreen(IUIScreenController controller, Transform screenTransform)
        {
            screenTransform.SetParent(transform, false);
        }

        
        public void RegisterScreen(string screenId, TScreenController controller)
        {
            if (!m_registeredScreens.ContainsKey(screenId)) {
                ProcessScreenRegister(screenId, controller);
            }
            else {
                Debug.LogError("[UILayerController] Screen controller already registered for id: " + screenId);
            }
        }
        
        public void UnregisterScreen(string screenId, TScreenController controller)
        {
            if (m_registeredScreens.ContainsKey(screenId)) {
                ProcessScreenUnregister(screenId, controller);
            }
            else {
                Debug.LogError("[UILayerController] Screen controller not registered for id: " + screenId);
            }
        }
        
        public void ShowScreenById(string screenId)
        {
            TScreenController ctl;
            if (m_registeredScreens.TryGetValue(screenId, out ctl)) {
                ShowScreen(ctl);
            }
            else {
                Debug.LogError("[UILayerController] Screen ID " + screenId + " not registered to this layer!");
            }
        }
        
        public void HideScreenById(string screenId)
        {
            TScreenController ctl;
            if (m_registeredScreens.TryGetValue(screenId, out ctl)) {
                HideScreen(ctl);
            }
            else {
                Debug.LogError("[AUILayerController] Could not hide Screen ID " + screenId + " as it is not registered to this layer!");
            }
        }
        
        public bool IsScreenRegistered(string screenId)
        {
            return m_registeredScreens.ContainsKey(screenId);
        }

        public virtual void HideAll(bool shouldAnimateWhenHiding = true)
        {
            foreach (var screen in m_registeredScreens) {
                screen.Value.Hide(shouldAnimateWhenHiding);
            }
        }

        protected virtual void ProcessScreenRegister(string screenId, TScreenController controller)
        {
            controller.ScreenId = screenId;
            m_registeredScreens.Add(screenId, controller);
            controller.OnScreenDestroyed += OnScreenDestroyed;
        }
        
        protected virtual void ProcessScreenUnregister(string screenId, TScreenController controller)
        {
            controller.OnScreenDestroyed -= OnScreenDestroyed;
            m_registeredScreens.Remove(screenId);
        }
        
        
        private void OnScreenDestroyed(IUIScreenController screen)
        {
            if (!string.IsNullOrEmpty(screen.ScreenId)
                && m_registeredScreens.ContainsKey(screen.ScreenId)) {
                UnregisterScreen(screen.ScreenId, (TScreenController) screen);
            }
        }
        
        public bool IsScreenVisibleById(string screenId)
        {
            TScreenController ctl;
            if (m_registeredScreens.TryGetValue(screenId, out ctl))
            {
                return ctl.IsVisible;
            }
            else {
                Debug.LogError("[AUILayerController] Could not indicates visibility Screen ID " + screenId + " as it is not registered to this layer!");
                return false;
            }
        }




        

    }
}