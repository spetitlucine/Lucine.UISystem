using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lucine.UISystem
{
    public abstract class UIPanel : UIPanel<UIPanelParameters>
    {
    }

    public class UIPanel<TParameters> : UIScreenController<TParameters>, IUIPanelController where TParameters : IUIPanelParameters
    {
    }
}