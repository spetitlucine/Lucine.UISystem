using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace Lucine.UISystem
{
    /// <summary>
    /// This class is just a singleton that controls all the UISystem
    /// You have use it to access all ui features, such as panel, windows and layers
    /// It requires a Canvas and a GraphicRaycaster
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    public class UIController : Singleton<UIController>
    {
        // the window layer
        UIWindowLayer m_WindowLayer;
        // the panel layer
        UIPanelLayer m_PanelLayer;
        // the canvas
        private Canvas m_MainCanvas;
        // the graphics raycaster
        private GraphicRaycaster m_GraphicRaycaster;
        // the camera to render UI. This camera must be set to render only layer UI and no clear (depth only)
        private Camera m_UICamera;


        /// <summary>
        /// On start initialize the UIController
        /// </summary>
        private void Start()
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the UIController
        /// cache the graphics ray caster, the maincanvas, the main_camera used to render the canvas
        /// </summary>
        protected virtual void Initialize()
        {
            m_GraphicRaycaster = GetComponent<GraphicRaycaster>();
            m_MainCanvas = GetComponent<Canvas>();
            m_UICamera = m_MainCanvas.worldCamera;
            
            m_WindowLayer = GetComponentInChildren<UIWindowLayer>();
            m_WindowLayer.Initialize();
            m_WindowLayer.HideAll();
            m_WindowLayer.DisableInteractionRequest += DisableRaycaster;
            m_WindowLayer.EnableInteractionRequest += EnableRaycaster;
            m_PanelLayer = GetComponentInChildren<UIPanelLayer>();
            m_PanelLayer.Initialize();
            m_PanelLayer.HideAll();
        }

        /// <summary>
        /// Open a window giving its id
        /// </summary>
        /// <param name="windowId">The id (name) of the window</param>
        public void OpenWindow(string windowId)
        {
            m_WindowLayer.ShowScreenById(windowId);
        }

        /// <summary>
        /// Close a window giving its id
        /// </summary>
        /// <param name="windowId">The id of the window</param>
        public void CloseWindow(string windowId)
        {
            m_WindowLayer.HideScreenById(windowId);
        }

        /// <summary>
        /// Close all opened windows()
        /// </summary>
        public void CloseAllWindows()
        {
            m_WindowLayer.HideAll();
        }

        /// <summary>
        /// Ask if the window id is visible (open)
        /// </summary>
        /// <param name="windowId">id of the window to test</param>
        /// <returns>true if visible</returns>
        public bool IsWindowVisible(string windowId)
        {
            return m_WindowLayer.IsVisible(windowId);
        }

        /// <summary>
        /// Show a panel (pendent of opening a window)
        /// </summary>
        /// <param name="panelId"></param>
        public void ShowPanel(string panelId)
        {
            m_PanelLayer.ShowScreenById(panelId);
        }

        /// <summary>
        /// Hide a panel givin its id
        /// </summary>
        /// <param name="panelId">id to show</param>
        public void HidePanel(string panelId)
        {
            m_PanelLayer.HideScreenById(panelId);
        }

        /// <summary>
        /// Hide all opened panels
        /// </summary>
        public void HideAllPanels()
        {
            m_PanelLayer.HideAll();
        }

        /// <summary>
        /// Is a panel visible
        /// </summary>
        /// <param name="panelId"></param>
        /// <returns></returns>
        public bool IsPanelVisible(string panelId)
        {
            return m_PanelLayer.IsPanelVisible(panelId);
        }

        /// <summary>
        /// Hide every window and panel
        /// </summary>
        public void HideAll()
        {
            CloseAllWindows();
            HideAllPanels();
        }


        /// <summary>
        /// callback to enable interactions
        /// </summary>
        public void EnableRaycaster()
        {
            m_GraphicRaycaster.enabled = true;
        }

        /// <summary>
        /// Callback to disable interactions
        /// </summary>
        public void DisableRaycaster()
        {
            m_GraphicRaycaster.enabled = false;
        }
        
        /// <summary>
        /// Ask if a screen (window or panel is registred with its id)
        /// </summary>
        /// <param name="screenId"></param>
        /// <returns></returns>
        public bool IsScreenRegistered(string screenId)
        {
            if (m_WindowLayer.IsScreenRegistered(screenId))
            {
                return true;
            }

            if (m_PanelLayer.IsScreenRegistered(screenId))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ask if a screen is registred and return its type
        /// </summary>
        /// <param name="screenId">id to test</param>
        /// <param name="type">returned type</param>
        /// <returns>true if registred in this case type is valid</returns>
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