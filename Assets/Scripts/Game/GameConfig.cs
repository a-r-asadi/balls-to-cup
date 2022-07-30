using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : ScriptableObject
{
    [SerializeField] private float rotationSensibility;

    [SerializeField] private float ballMaxVelocity;
    
    [SerializeField] private int uniqueLevelsCount;
    [SerializeField] private int overrideLevelNumber;

    [SerializeField] private int minBallsCount;
    [SerializeField] private int maxBallsCount;
    
    [SerializeField] private Material[] ballMaterials;

    public float RotationSensibility => rotationSensibility;
    
    public int UniqueLevelsCount => uniqueLevelsCount;

    public int OverrideLevelNumber => overrideLevelNumber;

    public int MINBallsCount => minBallsCount;

    public int MAXBallsCount => maxBallsCount;

    public Material[] BallMaterials => ballMaterials;

    public float BallMaxVelocity => ballMaxVelocity;
}