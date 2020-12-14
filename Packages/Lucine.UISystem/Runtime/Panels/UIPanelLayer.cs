
namespace Lucine.UISystem
{
    /// <summary>
    /// Panel Layer
    /// Panel is just like a screen so there's not a lot of overriding 
    /// </summary>
    public class UIPanelLayer : UILayerController<IUIPanelController>
    {
        protected override void ShowScreen(IUIPanelController screen)
        {
            screen.Show();
        }

        public override void ShowScreen<TParameters>(IUIPanelController screen, TParameters parameters)
        {
            screen.Show(parameters);
        }

        protected override void HideScreen(IUIPanelController screen)
        {
            screen.Hide();
        }

        public bool IsPanelVisible(string panelId)
        {
            return IsScreenVisibleById(panelId);
        }
    }
}