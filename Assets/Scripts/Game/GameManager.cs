using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameConfig config;
    
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
        LevelGenerator.instance.SetNextLevel();
    }

    public void Fail()
    {
        UIManager.instance.ShowFailMenu();
        currentState = State.Failed;
    }

    public void Restart()
    {
        UIManager.instance.Fade();
        StartCoroutine(RestartInternal());
    }

    private IEnumerator RestartInternal()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        
        UIManager.instance.HideFailMenu();
        LevelGenerator.instance.Reset();
        LevelGenerator.instance.OnRestart();
        currentState = State.Playing;
    }

    public void SetNextLevel()
    {
        UIManager.instance.Fade();
        StartCoroutine(SetNextLevelInternal());
    }
    
    private IEnumerator SetNextLevelInternal()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        
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
