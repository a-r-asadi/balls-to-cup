using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshPro text;
    private List<Ball> balls;

    private int minCount;

    private bool winFlag;

    public int InsideBallsCount => balls.Count;

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
            UpdateText();

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

    private void UpdateText()
    {
        text.text = $"{balls.Count}/{minCount}";
    }

    private IEnumerator SetWin()
    {
        yield return new WaitForSecondsRealtime(1f);

        if (GameManager.instance.GetState() == GameManager.State.Playing &&
            balls.Count >= minCount)
        {
            GameManager.instance.Win();
        }
    }
}
