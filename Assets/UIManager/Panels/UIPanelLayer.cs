using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lucine.UISystem
{
    public class UIPanelLayer : UILayerController<IUIPanelController>
    {
         public override void ShowScreen(IUIPanelController screen)
        {
            screen.Show();
        }

        public override void ShowScreen<TParameters>(IUIPanelController screen, TParameters parameters)
        {
            screen.Show(parameters);
        }

        public override void HideScreen(IUIPanelController screen)
        {
            screen.Hide();
        }

        public bool IsPanelVisible(string panelId)
        {
            return IsScreenVisibleById(panelId);
        }
    }
}