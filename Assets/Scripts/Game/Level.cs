using System;
using UnityEngine;

public class Level : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float winPercentage;
    
    [SerializeField] private Tube tube;
    [SerializeField] private Cup cup;
    [SerializeField] private Transform ballsParent;

    public float WinPercentage => winPercentage;
    
    public Tube Tube => tube;
    
    public Cup Cup => cup;

    public Transform BallsParent => ballsParent;
}
