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
    [SerializeField] private int randomSeed;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float scale;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    [Header("Crate Placement")]
    [SerializeField] private GameObject CratePrefab;
    [SerializeField] private Transform ObstacleTransform;
    [SerializeField] private float CratePercentageChance;

    [Header("Player")]
    [SerializeField] private Player PlayerPrefab;
    [SerializeField] private Vector3 PlayerStartPositon;

    [Header("Monster")]
    [SerializeField] private Monster MonsterPrefab;
    [SerializeField] private int NumberOfMonsters;

    /* -------- Internal Variables --------- */
    private System.Random Random;

    private void Start()
    {
        Random = new System.Random(randomSeed);
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        int yModifier;
        int xModifier = yModifier = 1;

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
                if (noise > CratePercentageChance)
                {
                    Vector3 pos = new Vector3(i * xModifier, j * yModifier, 0f);
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
        }
    }

    private Vector2 GetRandomAvailableVectorInLevel()
    {
        int startX, endX, startY, endY;
        if (StartAreaBottomRight.x > width)
        {
            startX = width;
            endX = StartAreaBottomRight.x;
        }
        else
        {
            startX = StartAreaBottomRight.x;
            endX = width;
        }
        if (StartAreaBottomRight.y > height)
        {
            startY = height;
            endY = StartAreaBottomRight.y;
        }
        else
        {
            startY = StartAreaBottomRight.y;
            endY = height;
        }

        int monsterX = Random.Next(startX, endX);
        int monsterY = Random.Next(startY, endY);
        Vector2 RandomVector = new Vector2(monsterX, monsterY);

        if (Physics2D.OverlapCircle(RandomVector, 0.2f))
        {
            RandomVector = GetRandomAvailableVectorInLevel();
        }

        return RandomVector;
    }

    private void ClearLevel()
    {
        foreach(Transform child in ObstacleTransform)
        {
            Destroy(child.gameObject);
        }

        foreach (Player _player in FindObjectsOfType<Player>())
            Destroy(_player.gameObject);

        foreach (Monster _monster in FindObjectsOfType<Monster>())
            Destroy(_monster.gameObject);

        foreach (Bomb _bomb in FindObjectsOfType<Bomb>())
            Destroy(_bomb.gameObject);
    }

    public void ReplayLevel()
    {
        ClearLevel();
        GenerateLevel();
    }

    public void NextLevel()
    {
        ClearLevel();
        randomSeed++;
        Random = new System.Random(randomSeed);
        NumberOfMonsters++;
        GenerateLevel();
    }
}
