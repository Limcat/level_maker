using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ToolType
{
    None,
    Eraser,
    Delete
}

[CreateAssetMenu(fileName = "Tool", menuName = "LevelBuilding/Create Tool")]
public class BuildingTool : BuildingObjectBase
{
    [SerializeField] private ToolType toolType;
    public void Use(Vector3Int pos)
    {
        ToolController tc = ToolController.GetInstance();
        switch(toolType)
        {
            case ToolType.Eraser:
                tc.Eraser(pos);
                break;
            case ToolType.Delete:
                tc.Delete();
                break;
            default:
                break;
        }
    }
}
