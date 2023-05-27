using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class BuildingCreator : MonoBehaviour
{
    [SerializeField] Tilemap previewMap, tilemap;
    TileBase tileBase;
    BuildingObjectBase selectedObj;
    [SerializeField] BuildingObjectBase dirtBlock, sandBlock;
    [SerializeField] Dropdown background;

    Vector2 mousePos;
    Vector3Int currentGridPos;
    Vector3Int lastGridPos;

    public void OnMousePosition(InputValue v)
    {
        mousePos = v.Get<Vector2>();
    }

    public void OnMouseLeftClick()
    {
        Debug.Log("Mouse left click");
        if (selectedObj != null && !EventSystem.current.IsPointerOverGameObject())
        {
            HandleItem();
            Debug.Log(currentGridPos);
        }
    }

    public void OnMouseRightClick()
    {
        Debug.Log("Mouse right click");
        SelectedObj = null;
    }

    public List<Database.Tile> GetTiles()
    {
        List<Database.Tile> TilePositions = new List<Database.Tile>();

        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.min.x; x < bounds.max.x; x++)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                TileBase tileBase = tilemap.GetTile(new Vector3Int(x, y, 0));
                if (tileBase != null)
                {
                    var tile = new Database.Tile();

                    switch (tileBase.name)
                    {
                        case "Dirt Tile Rule":
                            tile.tileName = Database.DIRT_BLOCK;
                            break;
                        case "Sand Tile Rule":
                            tile.tileName = Database.SAND_BLOCK;
                            break;
                        default:
                            break;
                    }

                    Debug.Log("Name:" + tile.tileName);
                    tile.posX = x;
                    tile.posY = y;

                    TilePositions.Add(tile);
                }
            }
        }

        return TilePositions;
    }
    

    //private void OnEnable()
    //{
    //    playerInput.Enable();
    //    playerInput.Gameplay.MousePosition.performed += OnMouseMove;
    //    playerInput.Gameplay.MouseLeftClick.performed += OnLeftClick;
    //    playerInput.Gameplay.MouseRightClick.performed += OnRightClick;
    //}

    //private void OnDisable()
    //{
    //    playerInput.Disable();
    //    playerInput.Gameplay.MousePosition.performed -= OnMouseMove;
    //    playerInput.Gameplay.MouseLeftClick.performed -= OnLeftClick;
    //    playerInput.Gameplay.MouseRightClick.performed -= OnRightClick;
    //}

    private BuildingObjectBase SelectedObj
    {
        set
        {
            selectedObj = value;
            tileBase = selectedObj != null ? selectedObj.TileBase : null;

            UpdatePreview();
        }
    }

    private void Start()
    {
        Database db = new Database();
        if (Database.mapName != null)
        {
            var tiles = db.LoadTiles(Database.mapName);
            foreach (Database.Tile tile in tiles)
            {
                switch (tile.tileName)
                {
                    case Database.DIRT_BLOCK:
                        tilemap.SetTile(tile.Position(), dirtBlock.TileBase);
                        break;
                    case Database.SAND_BLOCK:
                        tilemap.SetTile(tile.Position(), sandBlock.TileBase);
                        break;
                    default:
                        break;
                }
                Debug.Log("Pos:" + tile.Position() + "Name: " + tile.tileName);
            }
        }
    }

    private void Update()
    {
        if (selectedObj != null)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(mousePos);
            Vector3Int gridPos = previewMap.WorldToCell(pos);

            if (gridPos != currentGridPos)
            {
                lastGridPos = currentGridPos;
                currentGridPos = gridPos;

                UpdatePreview();
            }
        }
    }

    //private void OnMouseMove (InputAction.CallbackContext ctx)
    //{
    //    mousePos = ctx.ReadValue<Vector2>();
    //}

    //private void OnLeftClick(InputAction.CallbackContext ctx) 
    //{
    //    if (selectedObj != null && !EventSystem.current.IsPointerOverGameObject())
    //    {
    //        HandleDrawing();
    //    }
    //}

    //private void OnRightClick(InputAction.CallbackContext ctx) 
    //{
    //    SelectedObj = null;
    //}

    public void ObjectSelected(BuildingObjectBase obj)
    {
        SelectedObj = obj;
    }

    private void UpdatePreview()
    {
        previewMap.SetTile(lastGridPos, null);
        previewMap.SetTile(currentGridPos, tileBase);
    }

    private void HandleItem()
    {
        //if (selectedObj.GetType() == typeof(BuildingTool))
        //{
        //    BuildingTool tool = (BuildingTool)selectedObj;
        //    tool.Use(currentGridPos);
        //}
        //else
        //{
        //    tilemap.SetTile(currentGridPos, tileBase);
        //}
        switch(selectedObj.Category)
        {
            case Category.Block:
                tilemap.SetTile(currentGridPos, tileBase);
                break;
            case Category.Decoration:
                tilemap.SetTile(currentGridPos, tileBase);
                break;
            case Category.EraseTool:
                tilemap.SetTile(currentGridPos, null);
                break;
            case Category.DeleteTool:
                tilemap.ClearAllTiles();
                break;
            default:
                break;
        }
    }
}
