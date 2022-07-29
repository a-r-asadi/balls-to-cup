using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : UIMenu
{
    public void OnContinueClick()
    {
        GameManager.instance.SetNextLevel();
    }
}
