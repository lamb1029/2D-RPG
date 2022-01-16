using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapCollision : MonoBehaviour
{
    public Tilemap _tilemap;

    void Start()
    {
        _tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        List<Vector3Int> blocked = new List<Vector3Int>();

        foreach(Vector3Int pos in _tilemap.cellBounds.allPositionsWithin)
        {
            TileBase tile = _tilemap.GetTile(pos);
            if (tile != null)
                blocked.Add(pos);
        }
    }
}
