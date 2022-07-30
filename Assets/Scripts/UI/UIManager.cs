using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    
    [SerializeField] private UIMenu winMenu,
        failMenu,
        fadeMenu;

    [SerializeField] private TMPro.TextMeshProUGUI levelNumberText;

    private void Awake()
    {
        this.SetInstance(ref instance);
    }

    public void SetLevelNumber(int level)
    {
        levelNumberText.text = $"Level {level}";
    }

    public void ShowWinMenu()
    {
        winMenu.Show();
    }
    
    public void ShowFailMenu()
    {
        failMenu.Show();
    }
    
    public void HideWinMenu()
    {
        winMenu.Hide();
    }
    
    public void HideFailMenu()
    {
        failMenu.Hide();
    }

    public void Fade()
    {
        fadeMenu.FadeInOut();
    }
}
