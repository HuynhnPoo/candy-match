using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class UIAnimationManager : SingletonBase<UIAnimationManager>
{
    public Tween ScaleRecTransform( RectTransform rect,Vector3 targetScale,float duraction,Ease? ease=null,Action oncomplete=null) { 
        return rect.DOScale(targetScale,duraction).SetEase(ease?? Ease.InBack).SetUpdate(true).OnComplete(()=>oncomplete?.Invoke()); 
    }  

    public Tween MoveRecTransform(RectTransform rect, Vector3 targetScale, float duraction, Ease? ease = null, Action oncomplete = null)
    {
        return rect.DOMove(targetScale, duraction).SetEase(ease ?? Ease.InBack).SetUpdate(true).OnComplete(() => oncomplete?.Invoke());
    }
}
