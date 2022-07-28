using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private Tube tube;
    [SerializeField] private Transform ballsParent;

    public Tube Tube => tube;

    public Transform BallsParent => ballsParent;
}
