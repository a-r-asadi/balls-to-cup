using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.3f);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void FadeInOut()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.3f).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        
    }
}
