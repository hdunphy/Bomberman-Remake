using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Constraints")]
    [SerializeField] private Vector2Int StartAreaTopLeft;
    [SerializeField] private Vector2Int StartAreaBottomRight;

    [Header("Perlin Noise Factors")]
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float scale;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    [Header("Crate Placement")]
    [SerializeField] private GameObject CratePrefab;
    [SerializeField] private Transform ObstacleTransform;

    [Header("Player")]
    [SerializeField] private Player PlayerPrefab;
    [SerializeField] private Vector3 PlayerStartPositon;

    [Header("Monster")]
    [SerializeField] private Monster MonsterPrefab;
    [SerializeField] private int NumberOfMonsters;

    /* -------- Internal Variables --------- */
    //private List<Vector2> CrateStartPositions;

    private void Start()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        //CrateStartPositions = new List<Vector2>();

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
        for (int i = 0; i <= Mathf.Abs(width); i++)
        {
            for (int j = 0; j <= Mathf.Abs(height); j++)
            {
                float x = (float)i / width * scale + offsetX;
                float y = (float)j / height * scale + offsetY;
                float noise = Mathf.PerlinNoise(x, y);
                if (noise > 0.5f)
                {
                    Vector3 pos = new Vector3(i * xModifier, j * yModifier, 0f);
                    //CrateStartPositions.Add(pos);
                    //Debug.Log($"(i, j) ({i}, {j})\n{pos}");
                    Instantiate(CratePrefab, pos, Quaternion.identity, ObstacleTransform);
                }
            }
        }

        Collider2D[] colliders = Physics2D.OverlapAreaAll(StartAreaTopLeft, StartAreaBottomRight);
        foreach(Collider2D collider in colliders)
        {
            if (collider.CompareTag("Crate"))
            {
                Destroy(collider.gameObject);
            }
        }

        Instantiate(PlayerPrefab, PlayerStartPositon, Quaternion.identity);

        for(int i = 0; i < NumberOfMonsters; i++)
        {
            Monster _monster = Instantiate(MonsterPrefab, GetRandomAvailableVectorInLevel(), Quaternion.identity);
            //_monster.GetPlayer();
        }
    }

    private Vector2 GetRandomAvailableVectorInLevel()
    {
        int monsterX = UnityEngine.Random.Range(StartAreaBottomRight.x, width);
        int monsterY = UnityEngine.Random.Range(StartAreaBottomRight.y, height);
        Vector2 RandomVector = new Vector2(monsterX, monsterY);

        if (Physics2D.OverlapCircle(RandomVector, 0.2f))
        {
            RandomVector = GetRandomAvailableVectorInLevel();
        }

        return RandomVector;
    }
}
