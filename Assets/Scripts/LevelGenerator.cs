using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private GameObject CratePrefab;
    [SerializeField] private Transform ObstacleTransform;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        int yModifier;
        int xModifier = yModifier = 1;
        //int startX = width > 0 ? 0 : width;
        //int startY = height > 0 ? 0 : height;
        //int endX = width > 0 ? width : 0;
        //int endY = height > 0 ? height : 0;
        if (width < 0)
            xModifier = -1;
        if (height < 0)
            yModifier = -1;
        for (int i = 0; i < Mathf.Abs(width); i++)
        {
            for (int j = 0; j < Mathf.Abs(height); j++)
            {
                float x = (float)i / width;
                float y = (float)j / height;
                float noise = Mathf.PerlinNoise(x, y);
                if (noise > 0.5f)
                {
                    Vector3 pos = new Vector3(i * xModifier, j * yModifier, 0f);
                    Debug.Log($"(i, j) ({i}, {j})\n{pos}");
                    Instantiate(CratePrefab, pos, Quaternion.identity, ObstacleTransform);
                }
            }
        }
    }
}
