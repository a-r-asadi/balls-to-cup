using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;

    [SerializeField] private int uniqueLevelsCount;

    [SerializeField] private GameObject ballPrefab;

    [SerializeField] private int minBallsCount,
        maxBallsCount;
    
    [SerializeField] private Material[] ballMaterials;
    
    private int levelNumber;
    private int levelMinBallCount;

    private Level level;
    private List<Ball> levelBalls;

    private bool init;

    private bool failFlag;

    private void Awake()
    {
        this.SetInstance(ref instance);
    }
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("level"))
        {
            levelNumber = PlayerPrefs.GetInt("level");
        }
        else
        {
            levelNumber = 1;
            PlayerPrefs.SetInt("level", levelNumber);
        }
        
        Reset();
        GenerateLevel();
    }

    public void OnNextLevel()
    {
        levelNumber++;
        PlayerPrefs.SetInt("level", levelNumber);
        GenerateLevel();
    }
    
    public void OnRestart()
    {
        GenerateLevel();
    }

    public void Reset()
    {
        init = false;
        failFlag = false;
        levelBalls = new List<Ball>();
    }

    private void Update()
    {
        if (!init)
        {
            return;
        }
        
        if (GameManager.instance.GetState() != GameManager.State.Playing)
        {
            return;
        }
        
        for (int i = 0; i < levelBalls.Count; i++)
        {
            if (levelBalls[i].transform.position.y > level.Cup.transform.parent.position.y)
            {
                failFlag = false;
                return;
            }
        }

        if (failFlag)
        {
            return;
        }
        
        if (level.Cup.InsideBallsCount < levelMinBallCount)
        {
            failFlag = true;
            StartCoroutine(SetFail());
        }
    }

    private void GenerateLevel()
    {
        if (level)
        {
            Destroy(level.gameObject);
        }

        UIManager.instance.SetLevelNumber(levelNumber);

        int commonLevelNumber = (levelNumber - 1) % uniqueLevelsCount + 1;
        string path = $"Levels/Level {commonLevelNumber}";
        
        level = Instantiate(Resources.Load<GameObject>(path), Vector3.zero, Quaternion.identity)
            .GetComponent<Level>();

        int ballsCount = new System.Random(levelNumber).Next(minBallsCount, maxBallsCount + 1) / 10 * 10;

        StartCoroutine(GenerateBalls(ballsCount));

        levelMinBallCount = (int) (ballsCount * level.WinPercentage);
        level.Cup.Init(levelMinBallCount);

        init = true;
    }

    private IEnumerator GenerateBalls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newBall = Instantiate(ballPrefab, 
                level.Tube.BaseCenterPosition + 0.1f * Random.onUnitSphere, 
                Quaternion.identity, 
                level.BallsParent);

            int randomColorIndex = Random.Range(0, ballMaterials.Length);
            newBall.GetComponent<Ball>().SetMaterial(ballMaterials[randomColorIndex]);
            levelBalls.Add(newBall.GetComponent<Ball>());

            yield return new WaitForSeconds(0.3f / count);
        }
    }
    
    private IEnumerator SetFail()
    {
        yield return new WaitForSeconds(1f);
        
        if (GameManager.instance.GetState() == GameManager.State.Playing &&
            level.Cup.InsideBallsCount < levelMinBallCount &&
            failFlag)
        {
            GameManager.instance.Fail();
        }
    }
}
