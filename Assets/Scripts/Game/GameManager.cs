using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    private void Awake()
    {
        this.SetInstance(ref instance);
    }

    private State currentState;

    public State GetState()
    {
        return currentState;
    }

    public void Win()
    {
        UIManager.instance.ShowWinMenu();
        currentState = State.Won;
    }

    public void Fail()
    {
        UIManager.instance.ShowFailMenu();
        currentState = State.Failed;
    }

    public void Restart()
    {
        UIManager.instance.HideFailMenu();
        LevelGenerator.instance.Reset();
        LevelGenerator.instance.OnRestart();
        currentState = State.Playing;
    }

    public void SetNextLevel()
    {
        UIManager.instance.HideWinMenu();
        LevelGenerator.instance.Reset();
        LevelGenerator.instance.OnNextLevel();
        currentState = State.Playing;
    }
    
    public enum State
    {
        Playing,
        Won,
        Failed
    }
}
