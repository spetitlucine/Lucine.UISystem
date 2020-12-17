using System;
using DG.Tweening;
using Lucine.UISystem;
using UnityEngine;

/// <summary>
/// Sample class using DOTWeen to fade a Screen
/// Inherited from UITransition and implement Play function
/// </summary>
public class FadeTransition : UITransition
{
    [SerializeField] protected bool m_FadeIn;
    [SerializeField] protected float m_Time = 0.5f;
    [SerializeField] protected Ease m_Ease = Ease.Linear;

    /// <summary>
    /// The only function to implement
    /// </summary>
    /// <param name="target">The target transform to move</param>
    /// <param name="callWhenFinished">must be call when transition is over</param>
    public override void Play(Transform target, Action callWhenFinished)
    {
        // get the rectransform
        RectTransform rTransform = target as RectTransform;
        
        // add a canvas group if no one exist for the transform
        CanvasGroup canvasGroup = null;
        canvasGroup = rTransform.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        { 
            canvasGroup = rTransform.gameObject.AddComponent<CanvasGroup>();
        }

        // use DOTween to fade and callback call when complete
        canvasGroup.DOFade(m_FadeIn ? 1f : 0f, m_Time).SetEase(m_Ease).OnComplete(() => callWhenFinished() );
    }
}
