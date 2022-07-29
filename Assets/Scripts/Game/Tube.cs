using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    [SerializeField] private Transform baseCenterPlaceholder;

    public Vector3 BaseCenterPosition => baseCenterPlaceholder.position;
}
