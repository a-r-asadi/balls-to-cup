using System;
using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;

    [SerializeField] private GameObject ballPrefab;

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

        if (GameManager.instance.config.OverrideLevelNumber > 0)
        {
            levelNumber = GameManager.instance.config.OverrideLevelNumber;
            PlayerPrefs.SetInt("level", levelNumber);
        }
        
        Reset();
        GenerateLevel();
    }

    public void SetNextLevel()
    {
        levelNumber++;
        PlayerPrefs.SetInt("level", levelNumber);
    }

    public void OnNextLevel()
    {
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

        if (!HasAllBallsFellDown())
        {
            failFlag = false;
            return;
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

    private bool HasAllBallsFellDown()
    {
        for (int i = 0; i < levelBalls.Count; i++)
        {
            if (levelBalls[i].transform.position.y > level.Cup.transform.parent.position.y)
            {
                return false;
            }
        }

        return true;
    }

    private void GenerateLevel()
    {
        if (level)
        {
            Destroy(level.gameObject);
        }

        UIManager.instance.SetLevelNumber(levelNumber);

        int commonLevelNumber = (levelNumber - 1) % GameManager.instance.config.UniqueLevelsCount + 1;
        string path = $"Levels/Level {commonLevelNumber}";
        
        level = Instantiate(Resources.Load<GameObject>(path), Vector3.zero, Quaternion.identity)
            .GetComponent<Level>();

        int minBallsCount = GameManager.instance.config.MINBallsCount;
        int maxBallsCount = GameManager.instance.config.MAXBallsCount;
        int ballsCount = new System.Random(levelNumber).Next(minBallsCount, maxBallsCount + 1) / 10 * 10;

        StartCoroutine(GenerateBalls(ballsCount));

        levelMinBallCount = (int) (ballsCount * level.WinPercentage);
        level.Cup.Init(levelMinBallCount);

        init = true;
    }

    private IEnumerator GenerateBalls(int count)
    {
        Material[] ballMaterials = GameManager.instance.config.BallMaterials;
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
