using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Cup : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro text;
    [SerializeField] private ParticleSystem starParticles;
    
    private List<Ball> balls;

    private int minCount;

    private bool winFlag;

    public int InsideBallsCount => balls.Count;

    private Tweener scaleTween;

    public void Init(int minCount)
    {
        balls = new List<Ball>();
        this.minCount = minCount;
        UpdateText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Ball>())
        {
            Ball newBall = other.GetComponentInParent<Ball>();
            if (balls.Contains(newBall))
            {
                return;
            }

            balls.Add(newBall);
            UpdateText(true);

            if (winFlag)
            {
                return;
            }

            if (GameManager.instance.GetState() == GameManager.State.Playing &&
                balls.Count >= minCount)
            {
                winFlag = true;
                StartCoroutine(SetWin());
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<Ball>())
            {
                Ball newBall = other.GetComponentInParent<Ball>();
                if (!balls.Contains(newBall))
                {
                    return;
                }
    
                balls.Remove(newBall);
                UpdateText();
            }
        }

    private void UpdateText(bool hasEffect = false)
    {
        text.text = $"{balls.Count}/{minCount}";

        if (hasEffect)
        {
            scaleTween?.Kill();
            text.transform.localScale = Vector3.one;
            scaleTween = text.transform.DOScale(1.1f, 0.15f).SetLoops(2, LoopType.Yoyo);
        }
    }

    private IEnumerator SetWin()
    {
        starParticles.gameObject.SetActive(true);
        
        yield return new WaitForSecondsRealtime(1f);

        if (GameManager.instance.GetState() == GameManager.State.Playing &&
            balls.Count >= minCount)
        {
            GameManager.instance.Win();
        }
    }
}
