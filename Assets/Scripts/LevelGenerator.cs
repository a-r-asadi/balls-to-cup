using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;

    [SerializeField] private int minBallsCount,
        maxBallsCount;
    
    [SerializeField] private Material[] ballMaterials;
    
    private int levelNumber;

    private Level level;

    private void Start()
    {
        levelNumber = 1;
        GenerateLevel();
    }

    public void OnWin()
    {
        levelNumber++;
        
        GenerateLevel();
    }
    
    private void GenerateLevel()
    {
        string path = $"Levels/Level {levelNumber}";
        level = Instantiate(Resources.Load<GameObject>(path), Vector3.zero, Quaternion.identity)
            .GetComponent<Level>();

        int ballsCount = new System.Random(levelNumber).Next(minBallsCount, maxBallsCount + 1);
        StartCoroutine(GenerateBalls(ballsCount));
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

            yield return new WaitForSeconds(0.3f / count);
        }
    }
}
