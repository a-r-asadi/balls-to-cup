using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameConfigAsset : MonoBehaviour
{
    [MenuItem("Assets/Create/Game Config")]
    public static void CreateAsset()
    {
        CustomAssetUtility.CreateAsset<GameConfig>();
    }
}
