using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MapEditor
{
#if UNITY_EDITOR

    [MenuItem("Tools/GenerateMapCollision")]
    private static void GenerateMap()
    {
        GameObject[] map = Resources.LoadAll<GameObject>("Prefabs/Map");

        foreach (GameObject go in map)
        {
            Tilemap tm = Util.FindChild<Tilemap>(go, "Collision", true);

            using (var writer = File.CreateText($"Assets/Resources/Map/{go.name}_Collision.txt"))
            {
                writer.WriteLine($"xMin : {tm.cellBounds.xMin}");
                writer.WriteLine($"xMax : {tm.cellBounds.xMax}");
                writer.WriteLine($"yMin : {tm.cellBounds.yMin}");
                writer.WriteLine($"yMax : {tm.cellBounds.yMax}");

                for (int y = tm.cellBounds.yMax; y >= tm.cellBounds.yMin; y--)
                {
                    for (int x = tm.cellBounds.xMin; x <= tm.cellBounds.xMax; x++)
                    {
                        TileBase tile = tm.GetTile(new Vector3Int(x, y, 0));
                        if (tile != null)
                            writer.Write("1");
                        else
                            writer.Write("0");
                    }
                    writer.WriteLine();
                }
            }
        }
    }

#endif
}

