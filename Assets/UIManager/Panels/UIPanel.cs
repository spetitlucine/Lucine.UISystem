
namespace Lucine.UISystem
{
    /// <summary>
    /// Panel with basic Panel Parameters
    /// </summary>
    public abstract class UIPanel : UIPanel<UIPanelParameters>
    {
    }

    /// <summary>
    /// Panel with specific panel parameters
    /// </summary>
    /// <typeparam name="TParameters"></typeparam>
    public class UIPanel<TParameters> : UIScreenController<TParameters>, IUIPanelController where TParameters : IUIPanelParameters
    {
    }
}