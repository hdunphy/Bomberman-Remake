using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDeathBehavior : MonoBehaviour
{
    private Tilemap tilemap;
    public TileBase TileBase;

    public void Die(Vector3 position, float radius)
    {
        List<Vector3> coverage = new List<Vector3>();

        for (int i = 0; i <= radius; i++)
            for (int j = 0; j <= radius; j++)
                coverage.Add(new Vector3(i, j, 0));

        foreach (Vector3 _position in coverage)
        {
            Vector3Int tileMapPosition = tilemap.WorldToCell(_position);
            Debug.Log($"Destroy tilemap at: {tileMapPosition}");
            //Vector3Int intPosition = new Vector3Int((int)position.x, (int)position.y, 0);
            tilemap.SetTile(tileMapPosition, null);
        }

    }

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        EventManager.Instance.ExplodeBomb += Die;
    }

    private void OnDestroy()
    {
        EventManager.Instance.ExplodeBomb -= Die;
    }


}
