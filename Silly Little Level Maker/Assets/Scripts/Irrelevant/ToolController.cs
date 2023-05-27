using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolController : Singleton<ToolController>
{
    List<Tilemap> tilemaps = new List<Tilemap>();
    private void Start()
    {
        List<Tilemap> maps = FindObjectsOfType<Tilemap>().ToList();
        maps.ForEach(map => { if (map.name != "PreviewMap") { tilemaps.Add(map); } });
    }
    public void Eraser(Vector3Int pos)
    {
        tilemaps.ForEach(map => { map.SetTile(pos, null); });
        Debug.Log("POG!");
    }
    public void Delete()
    {
        tilemaps.ForEach(map => { map.ClearAllTiles(); });
    }
}
