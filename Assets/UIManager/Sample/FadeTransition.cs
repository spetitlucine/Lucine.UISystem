using System;
using DG.Tweening;
using Lucine.UISystem;
using UnityEngine;

public class FadeTransition : UITransition
{
    [SerializeField] protected bool m_FadeIn;
    [SerializeField] protected float m_Time = 0.5f;
    [SerializeField] protected Ease m_Ease = Ease.Linear;

    public override void Play(Transform target, Action callWhenFinished)
    {
        RectTransform rTransform = target as RectTransform;
        
        CanvasGroup canvasGroup = null;
        canvasGroup = rTransform.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
          canvasGroup = rTransform.gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.DOFade(m_FadeIn ? 1f : 0f, m_Time).SetEase(m_Ease).OnComplete(() => callWhenFinished() );
    }
}
