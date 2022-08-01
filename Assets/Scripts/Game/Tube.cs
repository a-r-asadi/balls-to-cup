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

        string path;
        string pathAssetName = $"SVGs/{svgName}.svg";
#if UNITY_ANDROID
        path = FileUtils.GetPersistentDataPath(pathAssetName);
#else
        path = DFile.GetStreamingDataPath(pathAssetName);
#endif

        SVGHandler.ConvertToSplines(path);

        spline = LevelGenerator.instance.gameObject.GetComponentInChildren<Spline>();

        emptyTube.gameObject.SetActive(true);
        points = new List<Vector3>();

        GameObject splineComputerPrefab =
            (GameObject) Resources.Load("Prefabs/SplineComputer", typeof(GameObject));
        SplineComputer splineComputer = Instantiate(splineComputerPrefab, transform.Find("Model"))
            .GetComponent<SplineComputer>();

        float length = spline.GetSplineLength();
        float distanceTraveled = 0;
        bool reversed = spline.NextDataPoint(0).Position.y <
                        spline.NextDataPoint(length - 0.1f).Position.y;

        for (int i = 0; i < length; i++)
        {
            SplineData data = !reversed
                ? spline.NextDataPoint(distanceTraveled)
                : spline.NextDataPoint(length - 1 - distanceTraveled);

            if (i == 0)
            {
                firstPoint = data.Position;
            }

            Vector3 position = 0.04f * (data.Position - firstPoint);
            if (position.y < 0)
            {
                position.y *= -1;
            }

            position.y += emptyTube.transform.position.y + 0.25f;

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