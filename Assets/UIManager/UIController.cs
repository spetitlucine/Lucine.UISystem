using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace Lucine.UISystem
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class UIController : Singleton<UIController>
    {
        UIWindowLayer m_WindowLayer;
        UIPanelLayer m_PanelLayer;
        private Canvas m_MainCanvas;
        private GraphicRaycaster m_GraphicRaycaster;
        private Camera m_UICamera;


        private void Start()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            m_GraphicRaycaster = GetComponent<GraphicRaycaster>();
            m_MainCanvas = GetComponent<Canvas>();
            m_UICamera = m_MainCanvas.worldCamera;
            
            m_WindowLayer = GetComponentInChildren<UIWindowLayer>();
            m_WindowLayer.Initialize();
            m_WindowLayer.HideAll();
            m_PanelLayer = GetComponentInChildren<UIPanelLayer>();
            m_PanelLayer.Initialize();
            m_PanelLayer.HideAll();

            DontDestroyOnLoad(this);
        }

        public void OpenWindow(string windowId)
        {
            m_WindowLayer.ShowScreenById(windowId);
        }

        public void CloseWindow(string windowId)
        {
            m_WindowLayer.HideScreenById(windowId);
        }

        public void CloseAllWindows()
        {
            m_WindowLayer.HideAll();
        }

        public bool IsWindowVisible(string windowId)
        {
            return m_WindowLayer.IsVisible(windowId);
        }

        public void ReparentWindow(string windowId, Transform t)
        {
            
        }

        public void ShowPanel(string panelId)
        {
            m_PanelLayer.ShowScreenById(panelId);
        }

        public void HideAllPanels()
        {
            m_PanelLayer.HideAll();
        }

        public void HideAll()
        {
            CloseAllWindows();
            HideAllPanels();
        }

        public void HidePanel(string panelId)
        {
            m_PanelLayer.HideScreenById(panelId);
        }

        public bool IsPanelVisible(string panelId)
        {
            return m_PanelLayer.IsPanelVisible(panelId);
        }

        public void EnableRaycaster()
        {
            m_GraphicRaycaster.enabled = true;
        }

        public void DisableRaycaster()
        {
            m_GraphicRaycaster.enabled = false;
        }
        
        public bool IsScreenRegistered(string screenId) {
            if (m_WindowLayer.IsScreenRegistered(screenId)) {
                return true;
            }

            if (m_PanelLayer.IsScreenRegistered(screenId)) {
                return true;
            }

            return false;
        }

        public bool IsScreenRegistered(string screenId, out Type type)
        {
            if (m_WindowLayer.IsScreenRegistered(screenId))
            {
                type = typeof(IUIWindowController);
                return true;
            }

            if (m_PanelLayer.IsScreenRegistered(screenId))
            {
                type = typeof(IUIPanelController);
                return true;
            }

            type = null;
            return false;
        }
    }
}