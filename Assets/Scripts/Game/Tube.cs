using System;
using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class Tube : MonoBehaviour
{
    [SerializeField] private Transform baseCenterPlaceholder;

    public Vector3 BaseCenterPosition => baseCenterPlaceholder.position;

    [SerializeField] private GameObject emptyTube;

    [SerializeField] private string svgName;
    
    private Spline spline;
    private List<Vector3> points;

    private Vector3 firstPoint;

    private void Start()
    {
        GenerateTube();
    }

    public void GenerateTube()
    {
        if (svgName == "")
        {
            return;
        }
        
        SVGHandler.ConvertToSplines($"Assets/SVGs/{svgName}.svg");
        spline = LevelGenerator.instance.gameObject.GetComponentInChildren<Spline>();
        
        emptyTube.gameObject.SetActive(true);
        points = new List<Vector3>();

        GameObject splineComputerPrefab =
            (GameObject) Resources.Load("Prefabs/SplineComputer", typeof(GameObject));
        SplineComputer splineComputer = Instantiate(splineComputerPrefab, transform.Find("Model"))
            .GetComponent<SplineComputer>();
        
        float length = spline.GetSplineLength();
        float distanceTraveled = 0;
        for (int i = 0; i < length; i++)
        {
            SplineData data = spline.NextDataPoint(distanceTraveled);

            if (i == 0)
            {
                firstPoint = data.Position;
            }

            Vector3 position = 0.04f * (data.Position - firstPoint);
            points.Add(position);

            List<SplinePoint> splinePoints = new List<SplinePoint>();

            for (int n = 0; n < points.Count; n++)
            {
                splinePoints.Add(new SplinePoint(points[n]));
            }

            splineComputer.SetPoints(splinePoints.ToArray());
            distanceTraveled += 1f;

            splineComputer.Rebuild();
        }
    }
}
